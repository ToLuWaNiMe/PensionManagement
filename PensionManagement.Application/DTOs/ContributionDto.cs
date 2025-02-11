using PensionManagement.Domain.Enums;
using System;

namespace PensionManagement.Application.DTOs
{
    public class ContributionDto
    {
        public int MemberId { get; set; }
        public decimal Amount { get; set; }
        public ContributionType ContributionType { get; set; }
        public DateTime ContributionDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}
