using Bl.Api;
using Bl.Models;
using Dal.Services;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Bl.Services
{

    public class BlMeasurementService : IBlMeasurement
    {
        IDal dal;// = new();
        public BlMeasurementService(IDal d)
        {
            dal = d;
        }
        public static async Task AddToBlMeasurementListAsync(string jsonFileMeasurement, List<BlMeasurement> blMeasurements)
        {
            List<Measurement> measurements = await MeasurementService.ConvertFromJson(jsonFileMeasurement);

            // המרת התאריך משם הקובץ לאובייקט מסוג תאריך
            string fileName = Path.GetFileName(jsonFileMeasurement);
            string[] parts = fileName.Split('_');

            if (parts.Length >= 4) //לוודא ששם הקובץ תקין מכיל את כל הנתונים
            {
                int day = int.Parse(parts[1]);
                int month = int.Parse(parts[2]);
                int year = int.Parse(parts[3].Split('.')[0]);

                DateTime specificDate = new DateTime(year, month, day);

                // המרת כל Measurement ל-BlMeasurement והוספה לרשימה
                blMeasurements.AddRange(measurements.Select(m => new BlMeasurement
                {
                    date = specificDate,
                    measurementTime = m.measurementTime,
                    temperature = m.temperature,
                    amountOfRain = m.amountOfRain,
                    windSpeed = m.windSpeed
                }));
            }
        }

        public static async Task<Dictionary<string, List<BlMeasurement>>> ListOfMeasurementObjectsForTheStationAsync(
     Dictionary<string, List<string>> classificationOfFileByStation)
        {
            var stationMeasurement = new Dictionary<string, List<BlMeasurement>>();

            foreach (var kvp in classificationOfFileByStation)
            {
                string key = kvp.Key;         
                List<string> filePaths = kvp.Value;  // רשימת הקבצים עבור התחנה

                var blMeasurements = new List<BlMeasurement>();
                // הוספת המדידות עבור כל קובץ
                
                 filePaths.ForEach(jsonFilePath => { AddToBlMeasurementListAsync(jsonFilePath, blMeasurements); });

                stationMeasurement[key] = blMeasurements;
            }

            return stationMeasurement;
        }

        public Dictionary<string, BlSummaryOfMeasurement> SummaryOfMeasurementsForEachStation(Dictionary<string, List<BlMeasurement>> stationMeasurement)//סיכום מדידות עבור כל תחנה
        {
            Dictionary<string, BlSummaryOfMeasurement> DictionarySummaryOfMeasurement = new Dictionary<string, BlSummaryOfMeasurement>();

            foreach (var kvp in stationMeasurement)
            {
                string key = kvp.Key;              //   שם התחנה- המפתח
                List<BlMeasurement> values = kvp.Value;     //  רשימת אובייקטי המדידות עבור תחנה 

                BlSummaryOfMeasurement SummaryOfMeasurement = BlSummaryOfMeasurementService.SummaryForEachStation(key, values);
                DictionarySummaryOfMeasurement[key]= SummaryOfMeasurement;

            }
            return DictionarySummaryOfMeasurement;  

        }
    }
}
