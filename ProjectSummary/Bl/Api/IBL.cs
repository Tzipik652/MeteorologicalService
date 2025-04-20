using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bl.Models;

namespace Bl.Api
{
    public interface IBL
    {
        IBlSummaryOfMeasurement SummaryOfMeasurement { get; }
        IBlMeasuringStation MeasuringStation { get; }
      //  IBlMeasurement Measurement { get; }

    }
}
