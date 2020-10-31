using System;
using System.Collections.Generic;

namespace Amonic_Airlines.Models
{
    public partial class User
    {
        public User()
        {
            ActivityUser = new HashSet<ActivityUser>();
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Office { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsActive { get; set; }
        private int CountInvalidAuthAttempts { get; set; }

        public virtual Office OfficeNavigation { get; set; }
        public virtual ICollection<ActivityUser> ActivityUser { get; set; }
    }
}
