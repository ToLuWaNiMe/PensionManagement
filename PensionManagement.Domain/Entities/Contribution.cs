using System;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Domain.Entities
{
    public class Contribution
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public ContributionType ContributionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ContributionDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public bool IsValid { get; set; } = false;
        public virtual Member Member { get; set; } = null!;
    }

}
