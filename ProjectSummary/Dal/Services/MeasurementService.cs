using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class MeasurementService : IMeasurement
    {

        public static async Task<List<Measurement>> ConvertFromJson(string filePath)
        {
            try
            {
                string jsonContent = await File.ReadAllTextAsync(filePath);
                List<Measurement> listObjects = JsonSerializer.Deserialize<List<Measurement>>(jsonContent);
                return listObjects;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error reading or deserializing the file", ex);
            }
        }

    }
}
