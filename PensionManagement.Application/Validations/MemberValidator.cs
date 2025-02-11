using FluentValidation;
using PensionManagement.Application.DTOs;

namespace PensionManagement.Application.Validations
{
    public class MemberValidator : AbstractValidator<MemberDto>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(m => m.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(BeValidAge).WithMessage("Member must be between 18 and 70 years old.");

            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(m => m.PhoneNumber)
                .Matches(@"^\d{10,15}$").WithMessage("Phone number must be 10 to 15 digits.");
        }

        private bool BeValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            return age >= 18 && age <= 70;
        }
    }
}
