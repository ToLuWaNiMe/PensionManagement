using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensionManagement.Domain.Entities
{
    public class Employer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
