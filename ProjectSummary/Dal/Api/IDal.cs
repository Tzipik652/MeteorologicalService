using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Api
{
    public interface IDal
    {
        public IMeasuringStation MeasuringStation { get; }
        public ISummaryOfMeasurement SummaryOfMeasurement { get; }
        public IMeasurement Measurement { get; }
    }
}
