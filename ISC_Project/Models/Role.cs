using System;
using System.Collections.Generic;

namespace ISC_Project.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
