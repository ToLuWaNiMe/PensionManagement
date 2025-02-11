using FluentValidation;
using PensionManagement.Application.DTOs;

namespace PensionManagement.Application.Validations
{
    public class ContributionValidator : AbstractValidator<ContributionDto>
    {
        public ContributionValidator()
        {
            RuleFor(c => c.MemberId)
                .GreaterThan(0).WithMessage("Member ID is required.");

            RuleFor(c => c.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(c => c.ContributionType)
                .IsInEnum()
                .WithMessage("Invalid contribution type.");

            RuleFor(c => c.ContributionDate)
                .NotEmpty().WithMessage("Contribution date is required.")
                .Must(BeValidDate).WithMessage("Invalid contribution date.");

            RuleFor(c => c.ReferenceNumber)
                .NotEmpty().WithMessage("Reference number is required.")
                .Matches(@"^[A-Z0-9]{8,15}$").WithMessage("Invalid reference number format.");
        }

        private bool BeValidDate(DateTime date)
        {
            return date <= DateTime.Today;
        }
    }
}
