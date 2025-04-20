using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class SummaryOfMeasurementService : ISummaryOfMeasurement
    {

        MyContext _dataManager;
        public SummaryOfMeasurementService(MyContext? m)
        {
            _dataManager = m;
        }
        public async Task CreateAsync(SummaryOfMeasurement summaryOfMeasurement)
        {
            if (summaryOfMeasurement == null)
                throw new ArgumentNullException(nameof(summaryOfMeasurement), "summary of measurement cannot be null.");

            if (summaryOfMeasurement.StationNumber <= 0)
                throw new ArgumentException("Station number must be a positive integer.");

            if (summaryOfMeasurement.MaximumTemperature < summaryOfMeasurement.MinimumTemperature)
                throw new ArgumentException("The maximum temperature must be greater than the minimum temperature.");

            if (summaryOfMeasurement.MaximumRainfall < summaryOfMeasurement.MinimumRainfall)
                throw new ArgumentException("The maximum rainfall must be greater than the minimum rainfall.");

            

            // הוספה ושמירה
            await _dataManager.SummaryOfMeasurements.AddAsync(summaryOfMeasurement);
            await _dataManager.SaveChangesAsync();
        }

        public async Task UpdateAsync(SummaryOfMeasurement summaryOfMeasurement)
        {
            if (summaryOfMeasurement == null)
                throw new ArgumentNullException(nameof(summaryOfMeasurement), "summary of measurement cannot be null.");

            var summaryOfMeasurementToUpdate = await _dataManager.SummaryOfMeasurements
                .FindAsync(summaryOfMeasurement.StationNumber);

            if (summaryOfMeasurementToUpdate == null)
            {
                throw new Exception(
                    $"summary of measurement with station number {summaryOfMeasurement.StationNumber} does not exist.");
            }

            if (summaryOfMeasurement.MaximumTemperature < summaryOfMeasurement.MinimumTemperature)
                throw new ArgumentException("The maximum temperature must be greater than the minimum temperature.");

            if (summaryOfMeasurement.MaximumRainfall < summaryOfMeasurement.MinimumRainfall)
                throw new ArgumentException("The maximum rainfall must be greater than the minimum rainfall.");

            // עדכון השדות
            summaryOfMeasurementToUpdate.MaximumTemperature = summaryOfMeasurement.MaximumTemperature;
            summaryOfMeasurementToUpdate.MinimumTemperature = summaryOfMeasurement.MinimumTemperature;
            summaryOfMeasurementToUpdate.MaximumRainfall = summaryOfMeasurement.MaximumRainfall;
            summaryOfMeasurementToUpdate.MinimumRainfall = summaryOfMeasurement.MinimumRainfall;

            // שמירה
            await _dataManager.SaveChangesAsync();
        }


        public async Task<List<SummaryOfMeasurement>> GetAsync()
        {
            try
            {
                return _dataManager.SummaryOfMeasurements.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
