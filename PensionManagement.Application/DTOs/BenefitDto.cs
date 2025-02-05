using PensionManagement.Domain.Enums;

namespace PensionManagement.Application.DTOs
{
    public class BenefitDto
    {
        public Guid MemberId { get; set; }
        public BenefitType BenefitType { get; set; }
        public decimal Amount { get; set; }
    }
}
