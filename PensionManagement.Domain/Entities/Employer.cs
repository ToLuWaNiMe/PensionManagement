﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensionManagement.Domain.Entities
{
    public class Employer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public bool ActiveStatus { get; set; }
    }

}
