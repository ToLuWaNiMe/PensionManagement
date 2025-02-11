using System;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Domain.Entities
{
    public class Benefit
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public BenefitType BenefitType { get; set; }
        public DateTime CalculationDate { get; set; }
        public EligibilityStatus EligibilityStatus { get; set; }
        public decimal Amount { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
