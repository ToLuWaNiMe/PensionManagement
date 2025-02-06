using PensionManagement.Domain.Enums;

namespace PensionManagement.Application.DTOs
{
    public class MemberDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public MemberType MemberType { get; set; }
    }
}
