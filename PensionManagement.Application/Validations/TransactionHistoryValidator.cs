using PensionManagement.Application.DTOs;
using FluentValidation;

namespace PensionManagement.Application.Validations
{
    public class TransactionHistoryValidator : AbstractValidator<TransactionHistoryDto>
    {
        public TransactionHistoryValidator()
        {
            RuleFor(x => x.EntityType)
                .IsInEnum().WithMessage("Invalid Entity Name");

            RuleFor(x => x.EntityId)
                .GreaterThan(0).WithMessage("Entity ID must be greater than zero");

            RuleFor(x => x.ActionType)
                .IsInEnum().WithMessage("Invalid Action Type");

            RuleFor(x => x.ChangeDate)
                .NotEmpty().WithMessage("Change Date is required");
        }
    }
}
