using System;
using System.Collections;
using System.Collections.Generic;

namespace Amonic_Airlines_CORE.Models
{
    public partial class Office
    {
        public Office()
        {
            Users = new HashSet<User>();
        }

        public int OfficeCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
