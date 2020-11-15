using Amonic_Airlines_CORE.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amonic_Airlines.Models
{
    public class FlightSchedulesModelView : FlightSchedules
    {
        public string Color { get => IsCanceled ? "Red" : "BlueViolet"; }
        public decimal BussinessPrice { get => Math.Round(EconomyPrice * (decimal)1.35, 2); }
        public decimal FirstClassPrice { get => Math.Round(BussinessPrice * (decimal)1.30); }
        public string FromName { get => AmonicContext.GetContext().Airport.Find(FromAir).ShortName; }
        public string ToName { get => AmonicContext.GetContext().Airport.Find(ToAir).ShortName; }

        public FlightSchedulesModelView(FlightSchedules flightSchedules)
        {
            base.CodeOfFlight = flightSchedules.CodeOfFlight;
            base.DateTimeOfRace = flightSchedules.DateTimeOfRace;
            base.EconomyPrice = flightSchedules.EconomyPrice;
            base.FlightNumber = flightSchedules.FlightNumber;
            base.FromAir = flightSchedules.FromAir;
            base.ToAir = flightSchedules.ToAir;
            base.IsCanceled = flightSchedules.IsCanceled;
        }
    }
}
