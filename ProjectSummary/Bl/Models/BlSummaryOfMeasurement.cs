using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Models
{
    public class BlSummaryOfMeasurement
    {
        public int StationNumber { get; set; }

        public double MaximumTemperature { get; set; }

        public double MinimumTemperature { get; set; }

        public double MaximumRainfall { get; set; }

        public double MinimumRainfall { get; set; }
    }
}
