using System;
using System.Collections.Generic;

namespace lab4.DataAccess
{
    public partial class Role
    {
        public Role()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string? Role1 { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
