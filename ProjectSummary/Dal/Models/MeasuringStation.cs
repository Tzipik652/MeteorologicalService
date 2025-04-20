using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class MeasuringStation
{
    public int StationNumber { get; set; }

    public string StationAddress { get; set; } = null!;

    public string Town { get; set; } = null!;

    public string? StationManager { get; set; }
}
