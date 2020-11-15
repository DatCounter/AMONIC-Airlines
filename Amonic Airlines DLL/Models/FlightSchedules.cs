using System;
using System.Collections.Generic;

namespace Amonic_Airlines_CORE.Models
{
    public partial class FlightSchedules
    {
        public int FlightNumber { get; set; }
        public DateTime DateTimeOfRace { get; set; }
        public int FromAir { get; set; }
        public int ToAir { get; set; }
        public int CodeOfFlight { get; set; }
        public decimal EconomyPrice { get; set; }
        public bool IsCanceled { get; set; }

        public virtual Flight CodeOfFlightNavigation { get; set; }
        public virtual Airport FromAirNavigation { get; set; }
        public virtual Airport ToAirNavigation { get; set; }
    }
}
