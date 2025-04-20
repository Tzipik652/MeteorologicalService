using Bl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Api
{
    public interface IBlSummaryOfMeasurement
    {
        Task CreateAsync(string nameStation, List<BlMeasurement> listMeasurement);
        Task UpdateAsync(string nameStation, List<BlMeasurement> listMeasurement);
        Task<List<BlSummaryOfMeasurement>> GetAsync();
    }
}
