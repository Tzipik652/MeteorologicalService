using Bl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Api
{
    public interface IBlMeasuringStation 
    {
        Task CreateAsync(BlMeasuringStation item);
        Task UpdateAsync(BlMeasuringStation item);
        Task<List<BlMeasuringStation>> GetAsync();
        Task<Dictionary<string, List<string>>> ClassificationOfFilePathByStationNameAsync(string stationslistPath);
    }
}
