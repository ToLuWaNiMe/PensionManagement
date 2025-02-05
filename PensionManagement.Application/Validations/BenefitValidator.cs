using FluentValidation;
using PensionManagement.Application.DTOs;
using System;

namespace PensionManagement.Application.Validations
{
    public class BenefitValidator : AbstractValidator<BenefitDto>
    {
        public BenefitValidator()
        {
            RuleFor(b => b.MemberId).NotEmpty();
            RuleFor(b => b.BenefitType)
                .IsInEnum()
                .WithMessage("Invalid benefit type.");
            RuleFor(b => b.Amount)
                .GreaterThan(0)
                .WithMessage("Benefit amount must be greater than zero.");
        }
    }
}
