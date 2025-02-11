using System;
using System.Reflection;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public MemberType MemberType { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
        public virtual ICollection<Benefit> Benefits { get; set; } = new List<Benefit>();
    }

}
