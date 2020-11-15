using System;
using System.Collections.Generic;

namespace Amonic_Airlines_CORE.Models
{
    public partial class Flight
    {
        public Flight()
        {
            FlightSchedules = new HashSet<FlightSchedules>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<FlightSchedules> FlightSchedules { get; set; }
    }
}
