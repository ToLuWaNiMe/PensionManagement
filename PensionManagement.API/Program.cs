using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using PensionManagement.API;
using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;
using PensionManagement.Application.Mappings;
using PensionManagement.Application.Services;
using PensionManagement.Application.Validations;
using PensionManagement.Infrastructure.Middleware;
using PensionManagement.Infrastructure.Persistence;
using PensionManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation(); // Enables FluentValidation
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IContributionRepository, ContributionRepository>();
builder.Services.AddScoped<IBenefitRepository, BenefitRepository>();


// Register Validators
builder.Services.AddScoped<IValidator<MemberDto>, MemberValidator>();
builder.Services.AddScoped<IValidator<ContributionDto>, ContributionValidator>();
builder.Services.AddScoped<IValidator<BenefitDto>, BenefitValidator>();
builder.Services.AddScoped<IValidator<TransactionHistoryDto>, TransactionHistoryValidator>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Hangfire to use SQL Server
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer(); // Add Hangfire background job server

// Register the Background Service
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<ContributionService>();
builder.Services.AddScoped<BenefitService>();
builder.Services.AddScoped<BackgroundJobService>();
builder.Services.AddScoped<BenefitProcessingService>();
builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>(); // Register global error handling
app.UseHttpsRedirection();
// Start Hangfire Dashboard
app.UseHangfireDashboard();

app.MapControllers();
app.UseRouting();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var backgroundJobService = scope.ServiceProvider.GetRequiredService<BackgroundJobService>();
    var benefitProcessingService = scope.ServiceProvider.GetRequiredService<BenefitProcessingService>();

    //Schedule Daily Job for Contribution Validation
    RecurringJob.AddOrUpdate(
        "validate-contributions",
        () => backgroundJobService.ValidateContributions(),
        Cron.Daily);

    //Schedule Monthly Job for Benefit Processing
    RecurringJob.AddOrUpdate(
        "process-benefit-eligibility",
        () => benefitProcessingService.ProcessBenefitEligibility(),
        Cron.Monthly);
}

app.Run();
