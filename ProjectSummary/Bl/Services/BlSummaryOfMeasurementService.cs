using Bl.Api;
using Bl.Models;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bl.Services
{
    // איך מבצעים את הפעולות שהוגדרו
    public class BlSummaryOfMeasurementService : IBlSummaryOfMeasurement
    {
        private readonly IDal dal;

        public BlSummaryOfMeasurementService(IDal d)
        {
            dal = d;
        }

        public async Task CreateAsync(string nameStation, List<BlMeasurement> listMeasurement)
        {
            // בדיקת תקינות
            if (string.IsNullOrWhiteSpace(nameStation))
                throw new ArgumentException("Station name cannot be null, empty, or whitespace.");

            if (listMeasurement == null || listMeasurement.Count == 0)
                throw new ArgumentNullException(nameof(listMeasurement), "List of measurements cannot be null or empty.");

            // יוצרים את סיכום המדידות כ- BlSummaryOfMeasurement
            BlSummaryOfMeasurement summaryOfMeasurement = SummaryForEachStation(nameStation, listMeasurement);

            // ווידוא שאין כבר נתונים לאותה תחנה
            var existingSummaries = await dal.SummaryOfMeasurement.GetAsync();
            if (existingSummaries.Any
                (c => c.StationNumber == summaryOfMeasurement.StationNumber))
            {
                await UpdateAsync(nameStation, listMeasurement);
            }
            //  – יוצרים סיכום חדש ב-DAL
            else
            {
                await dal.SummaryOfMeasurement.CreateAsync(GetSummaryOfMeasurement(summaryOfMeasurement));

            }
        }
        public async Task UpdateAsync(string nameStation, List<BlMeasurement> listMeasurement)
        {
            // בדיקת תקינות
            if (string.IsNullOrWhiteSpace(nameStation))
                throw new ArgumentException("Station name cannot be null, empty, or whitespace.");

            if (listMeasurement == null || listMeasurement.Count == 0)
                throw new ArgumentNullException(nameof(listMeasurement), "List of measurements cannot be null or empty.");

            BlSummaryOfMeasurement summaryOfMeasurement = SummaryForEachStation(nameStation, listMeasurement);

            await dal.SummaryOfMeasurement.UpdateAsync(GetSummaryOfMeasurement(summaryOfMeasurement));
        }
        // המרת BlSummaryOfMeasurement ל-SummaryOfMeasurement של DAL
        private SummaryOfMeasurement GetSummaryOfMeasurement(BlSummaryOfMeasurement summaryOfMeasurement) =>
            new SummaryOfMeasurement
            {
                StationNumber = summaryOfMeasurement.StationNumber,
                MaximumTemperature = summaryOfMeasurement.MaximumTemperature,
                MinimumTemperature = summaryOfMeasurement.MinimumTemperature,
                MaximumRainfall = summaryOfMeasurement.MaximumRainfall,
                MinimumRainfall = summaryOfMeasurement.MinimumRainfall,
            };


        public async Task<List<BlSummaryOfMeasurement>> GetAsync()
        {
            // שליפת רשימת הסיכומים בגרסה אסינכרונית
            List<SummaryOfMeasurement> list = await dal.SummaryOfMeasurement.GetAsync();

            // המרה ל-List<BlSummaryOfMeasurement>
            List<BlSummaryOfMeasurement> blSummaryOfMeasurements = list.Select(summary => new BlSummaryOfMeasurement
            {
                StationNumber = summary.StationNumber,
                MaximumTemperature = summary.MaximumTemperature,
                MinimumTemperature = summary.MinimumTemperature,
                MaximumRainfall = summary.MaximumRainfall,
                MinimumRainfall = summary.MinimumRainfall,
            }).ToList();

            return blSummaryOfMeasurements;
        }

       
        public static BlSummaryOfMeasurement SummaryForEachStation(string nameStation, List<BlMeasurement> listMeasurement)
        {
            BlSummaryOfMeasurement summaryOfMeasurement = new BlSummaryOfMeasurement
            {
                StationNumber = Convert.ToInt32(nameStation),
                MaximumTemperature = listMeasurement.Max(m => m.temperature),
                MinimumTemperature = listMeasurement.Min(m => m.temperature),
                MaximumRainfall = listMeasurement.Max(m => m.amountOfRain),
                MinimumRainfall = listMeasurement.Min(m => m.amountOfRain)
            };

            return summaryOfMeasurement;
        }
    }
}
