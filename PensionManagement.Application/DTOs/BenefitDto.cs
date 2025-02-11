using PensionManagement.Domain.Enums;

namespace PensionManagement.Application.DTOs
{
    public class BenefitDto
    {
        public int MemberId { get; set; }
        public BenefitType BenefitType { get; set; }
        public DateTime CalculationDate { get; set; }
        public decimal Amount { get; set; }
    }
}
