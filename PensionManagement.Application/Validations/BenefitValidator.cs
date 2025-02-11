using FluentValidation;
using PensionManagement.Application.DTOs;
using System;

namespace PensionManagement.Application.Validations
{
    public class BenefitValidator : AbstractValidator<BenefitDto>
    {
        public BenefitValidator()
        {
            RuleFor(b => b.MemberId)
                .GreaterThan(0).WithMessage("Member ID is required.");

            RuleFor(b => b.BenefitType)
                .NotEmpty().WithMessage("Benefit type is required.");

            RuleFor(b => b.CalculationDate)
                .NotEmpty().WithMessage("Calculation date is required.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Calculation date cannot be in the future.");

            RuleFor(b => b.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be zero or greater.");
        }
    }
}
