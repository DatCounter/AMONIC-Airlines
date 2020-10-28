using System;
using System.Collections.Generic;

namespace Amonic_Airlines_CORE.Models
{
    public partial class Offices
    {
        public Offices()
        {
            Users = new HashSet<Users>();
        }

        public int OfficeCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
