using System;
using System.Collections.Generic;

namespace lab4.DataAccess
{
    public partial class Employee
    {
        public Employee()
        {
            Roles = new HashSet<Role>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Role> Roles { get; set; }
    }
}
