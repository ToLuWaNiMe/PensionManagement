using System;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Domain.Entities
{
    public class Contribution
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MemberId { get; set; } // Foreign Key
        public decimal Amount { get; set; }
        public ContributionType ContributionType { get; set; } // Enum for Monthly/Voluntary
        public DateTime ContributionDate { get; set; } = DateTime.UtcNow;
        public bool IsProcessed { get; set; } = false;
        public bool? IsValid { get; set; }
        public Member Member { get; set; } // Navigation property
    }
}
