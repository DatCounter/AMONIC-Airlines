using Amonic_Airlines.Models;
using Amonic_Airlines_CORE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Amonic_Airlines.Services
{
    public class ImportFlightsService
    {
        public int SuccessfulChanges { get; set; } = 0;
        public int DuplicateRecords { get; set; } = 0;
        public int RecordWithMissingFields { get; set; } = 0;

        public List<string[]> StartImport(string path)
        {
            List<string[]> dubplicatesList = new List<string[]>();

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var item = reader.ReadLine().Split(',');
                    if (item.Length != 9)
                    {
                        RecordWithMissingFields++;
                        continue;
                    }
                    for (int i = 0; i < item.Length; i++)
                    {
                        item[i] = item[i].Trim()
                            .ToLower();
                    }
                    //Узнать, что делать с дублированием PK в расписании полётов
                    if (item[0] == "add")
                    {
                        if (DateTime.TryParse($"{item[1]} {item[2]}", out DateTime dateTimeOfFlight))
                        {
                            if (Int32.TryParse(item[3], out int NumberOfRace))
                            {
                                if (AmonicContext.GetContext().FlightSchedules.FirstOrDefault(FS => FS.FlightNumber == NumberOfRace
                                                                                                && FS.DateTimeOfRace == dateTimeOfFlight) == null)
                                {
                                    if (Int32.TryParse(item[6], out int CodeOfFlight))
                                    {
                                        var fromAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[4]);
                                        var toAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[5]);
                                        var Flight = AmonicContext.GetContext().Flight.FirstOrDefault(a => a.Id == CodeOfFlight);
                                        if (Flight != null)
                                        {
                                            if (fromAir == null && item[4].Length == 3
                                                && item[4].IndexOfAny(new char[] {
                                                            '1', '2','3','4','5','6','7','8','9','0'}) == -1)
                                            {
                                                fromAir = new Airport()
                                                {
                                                    ShortName = item[4].ToUpper()
                                                };
                                                AmonicContext.GetContext().Add(fromAir);
                                                AmonicContext.GetContext().SaveChanges();
                                                fromAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[4]);
                                            }
                                            if (toAir == null && item[5].Length == 3 && item[4].IndexOfAny(new char[] {
                                        '1', '2','3','4','5','6','7','8','9','0'}) == -1)
                                            {
                                                toAir = new Airport()
                                                {
                                                    ShortName = item[5].ToUpper()
                                                };
                                                AmonicContext.GetContext().Add(toAir);
                                                AmonicContext.GetContext().SaveChanges();
                                                toAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[5]);
                                            }
                                            if (decimal.TryParse(item[7], out decimal price))
                                            {
                                                if (fromAir.ShortName != toAir.ShortName)
                                                {
                                                    if (item[8] == "ok" || item[8] == "canceled")
                                                    {
                                                        FlightSchedules flightSchedules = new FlightSchedules()
                                                        {
                                                            FlightNumber = NumberOfRace,
                                                            DateTimeOfRace = dateTimeOfFlight,
                                                            FromAir = fromAir.Id,
                                                            ToAir = toAir.Id,
                                                            CodeOfFlight = Flight.Id,
                                                            EconomyPrice = price,
                                                            IsCanceled = !(item[8] == "ok")
                                                        };
                                                        AmonicContext.GetContext().FlightSchedules.Add(flightSchedules);
                                                        AmonicContext.GetContext().SaveChanges();
                                                        SuccessfulChanges++;
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    dubplicatesList.Add(item);
                                    DuplicateRecords++;
                                    continue;
                                }
                            }
                        }
                        RecordWithMissingFields++;
                        continue;
                    }
                    else if (item[0] == "edit")
                    {
                        if (DateTime.TryParse($"{item[1]} {item[2]}", out DateTime dateTimeOfFlight))
                        {
                            if (Int32.TryParse(item[3], out int NumberOfRace))
                            {
                                var flightSchedule = AmonicContext.GetContext().FlightSchedules.FirstOrDefault(FS => FS.FlightNumber == NumberOfRace
                                                                                                && FS.DateTimeOfRace == dateTimeOfFlight);
                                if (flightSchedule != null)
                                {
                                    if (Int32.TryParse(item[6], out int CodeOfFlight))
                                    {
                                        var fromAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[4]);
                                        var toAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[5]);
                                        var Flight = AmonicContext.GetContext().Flight.FirstOrDefault(a => a.Id == CodeOfFlight);
                                        if (Flight != null)
                                        {
                                            if (fromAir == null && item[4].Length == 3
                                                && item[4].IndexOfAny(new char[] {
                                                            '1', '2','3','4','5','6','7','8','9','0'}) == -1)
                                            {
                                                fromAir = new Airport()
                                                {
                                                    ShortName = item[4].ToUpper()
                                                };
                                                AmonicContext.GetContext().Add(fromAir);
                                                AmonicContext.GetContext().SaveChanges();
                                                fromAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[4]);
                                            }
                                            if (toAir == null && item[5].Length == 3 && item[5].IndexOfAny(new char[] {
                                        '1', '2','3','4','5','6','7','8','9','0'}) == -1)
                                            {
                                                toAir = new Airport()
                                                {
                                                    ShortName = item[5].ToUpper()
                                                };
                                                AmonicContext.GetContext().Add(toAir);
                                                AmonicContext.GetContext().SaveChanges();
                                                toAir = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == item[5]);
                                            }
                                            if (decimal.TryParse(item[7], out decimal price))
                                            {
                                                if (fromAir.ShortName != toAir.ShortName)
                                                {
                                                    if (item[8] == "ok" || item[8] == "canceled")
                                                    {
                                                        flightSchedule.FromAir = fromAir.Id;
                                                        flightSchedule.ToAir = toAir.Id;
                                                        flightSchedule.CodeOfFlight = Flight.Id;
                                                        flightSchedule.EconomyPrice = price;
                                                        flightSchedule.IsCanceled = !(item[8] == "ok");

                                                        AmonicContext.GetContext().SaveChanges();
                                                        SuccessfulChanges++;
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    DuplicateRecords++;
                                    continue;
                                }
                            }
                        }
                        RecordWithMissingFields++;
                        continue;
                    }
                    else
                    {
                        RecordWithMissingFields++;
                        continue;
                    }
                }
            }

            return dubplicatesList;
        }
       
    }
}
