using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Amonic_Airlines.Services
{
    public class ImportFlightsService
    {
        public static void StartImport(string path)
        {
            using (var reader = new StreamReader(path))
            {
                while (reader.EndOfStream)
                {

                }
            }
        }
    }
}
