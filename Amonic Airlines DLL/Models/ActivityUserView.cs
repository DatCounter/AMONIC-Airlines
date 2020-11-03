using System;
using System.Collections.Generic;
using System.Text;

namespace Amonic_Airlines_CORE.Models
{
    public class ActivityUserView : ActivityUser
    {
        public string Color { get; set; }
        public ActivityUserView(ActivityUser au)
        {
            this.Email = au.Email;
            this.LoginDate = au.LoginDate;
            this.LogoutDate = au.LogoutDate;
            this.TimeSpent = au.TimeSpent;
            this.UnsuccessfulLogoutReason = au.UnsuccessfulLogoutReason;
            this.Color = au.LogoutDate == null ? "Red" : "IndianBlue";
        }
    }
}
