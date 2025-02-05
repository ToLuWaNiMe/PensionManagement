using FluentValidation;
using PensionManagement.Application.DTOs;

namespace PensionManagement.Application.Validations
{
    public class MemberValidator : AbstractValidator<MemberDto>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.DateOfBirth).NotEmpty()
                .LessThan(DateTime.UtcNow.AddYears(-18)) // Must be at least 18 years old
                .WithMessage("Member must be at least 18 years old.");
            RuleFor(m => m.PhoneNumber).NotEmpty().Matches(@"^\+?\d{10,15}$")
                .WithMessage("Invalid phone number format.");
        }
    }
}
