using System;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Domain.Entities
{
    public class Benefit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MemberId { get; set; } // Foreign Key
        public BenefitType BenefitType { get; set; }
        public decimal Amount { get; set; }
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow;
        public bool EligibilitySatus { get; set; } = false;
        public Member Member { get; set; } // Navigation property
    }
}
