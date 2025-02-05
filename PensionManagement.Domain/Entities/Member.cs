using System;
using System.Reflection;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public MemberType MemberType { get; set; } // Enum for Employee/Self-employed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
