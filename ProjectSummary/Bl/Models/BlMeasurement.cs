using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Models
{
    public class BlMeasurement
    {
        public DateTime date {  get; set; }
        public TimeSpan measurementTime {  get; set; }
        public double temperature {  get; set; }
        public double amountOfRain {  get; set; }
        public double windSpeed {  get; set; }

    }
}
