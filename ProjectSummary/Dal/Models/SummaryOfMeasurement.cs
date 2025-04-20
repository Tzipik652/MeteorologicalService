using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class SummaryOfMeasurement
{
    public int StationNumber { get; set; }

    public double MaximumTemperature { get; set; }

    public double MinimumTemperature { get; set; }

    public double MaximumRainfall { get; set; }

    public double MinimumRainfall { get; set; }
}
