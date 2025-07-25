﻿using System;
using System.Collections.Generic;

namespace ISC_Project.Models
{
    public partial class Campus
    {
        public int CampusesId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ManagerName { get; set; }
        public int? SchoolId { get; set; }

        public virtual SchoolProfile? School { get; set; }
    }
}
