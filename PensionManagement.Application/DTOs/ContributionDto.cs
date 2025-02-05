using PensionManagement.Domain.Enums;
using System;

namespace PensionManagement.Application.DTOs
{
    public class ContributionDto
    {
        public Guid MemberId { get; set; }
        public decimal Amount { get; set; }
        public ContributionType ContributionType { get; set; }
    }
}
