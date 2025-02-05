using FluentValidation;
using PensionManagement.Application.DTOs;

namespace PensionManagement.Application.Validations
{
    public class ContributionValidator : AbstractValidator<ContributionDto>
    {
        public ContributionValidator()
        {
            RuleFor(c => c.MemberId).NotEmpty();
            RuleFor(c => c.Amount)
                .GreaterThan(0)
                .WithMessage("Contribution amount must be greater than zero.");
            RuleFor(c => c.ContributionType)
                .IsInEnum()
                .WithMessage("Invalid contribution type.");
        }
    }
}
