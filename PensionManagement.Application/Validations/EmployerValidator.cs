using FluentValidation;
using PensionManagement.Application.DTOs;
using PensionManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensionManagement.Application.Validations
{
    public class EmployerValidator : AbstractValidator<EmployerDto>
    {
        public EmployerValidator()
        {
            RuleFor(e => e.CompanyName)
                .NotEmpty().WithMessage("Company name is required.");

            RuleFor(e => e.RegistrationNumber)
                .NotEmpty().WithMessage("Registration number is required.")
                .Matches(@"^[A-Z0-9]{8,15}$").WithMessage("Invalid registration number format.");
        }
    }
}
