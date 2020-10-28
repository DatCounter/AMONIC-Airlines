using System;
using System.Collections.Generic;

namespace Amonic_Airlines_CORE.Models
{
    public partial class ActivityUser
    {
        public string Email { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string UnsuccessfulLogoutReason { get; set; }
        public TimeSpan? TimeSpent { get; set; }

        public virtual Users EmailNavigation { get; set; }
    }
}
