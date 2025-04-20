using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Models
{
    public class BlMeasuringStation
    {

        public int StationNumber { get; set; }

        public string StationAddress { get; set; } = null!;

        public string Town { get; set; } = null!;

        public string? StationManager { get; set; }
        public List<BlMeasurement>? Measurements { get; set; } = null;


    }
}
