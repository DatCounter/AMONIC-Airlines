using System;
using System.Collections.Generic;

namespace Amonic_Airlines_CORE.Models
{
    public partial class Airport
    {
        public Airport()
        {
            FlightSchedulesFromAirNavigation = new HashSet<FlightSchedules>();
            FlightSchedulesToAirNavigation = new HashSet<FlightSchedules>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<FlightSchedules> FlightSchedulesFromAirNavigation { get; set; }
        public virtual ICollection<FlightSchedules> FlightSchedulesToAirNavigation { get; set; }
    }
}
