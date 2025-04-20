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
using System.IO;

namespace Bl.Services
{
    // איך מבצעים את הפעולות שהוגדרו
    public class BlMeasuringStationService : IBlMeasuringStation
    {
        // הזרקת תלויות
        private readonly IDal dal;

        public BlMeasuringStationService(IDal d)
        {
            dal = d;
        }

        // מחזירה מילון של { stationName => רשימת קבצים ששייכים לאותה תחנה } 
        public async Task<Dictionary<string, List<string>>> ClassificationOfFilePathByStationNameAsync(string stationslistPath)
        {
            // ודאי שהתיקיה קיימת
            if (!Directory.Exists(stationslistPath))
            {
                throw new Exception("The stations list path does not exist.");
            }

            // קבלת רשימת הקבצים בתיקיה 
            List<string> filesPath = new List<string>(Directory.GetFiles(stationslistPath));

            // יצירת מילון שבו המפתח הוא שם התחנה והערך הוא רשימת הקבצים
            Dictionary<string, List<string>> stationFiles = new Dictionary<string, List<string>>();

            // שליפת התחנות מה-DAL בגרסה אסינכרונית
            List<MeasuringStation> listOfStation = await dal.MeasuringStation.GetAsync();

            foreach (var filePath in filesPath)
            {
                // קבלת שם הקובץ בלבד (ללא הנתיב)
                string fileName = Path.GetFileName(filePath);

                // מציאת המיקום של הקו התחתון הראשון
                int underscoreIndex = fileName.IndexOf('_');
                if (underscoreIndex <= 0)
                {
                    throw new Exception($"File '{fileName}' does not follow the expected naming convention (missing underscore).");
                }

                // חלק לפני הקו התחתון (למשל "123" אם הקובץ הוא "123_2023-02-16.txt")
                string stationName = fileName.Substring(0, underscoreIndex);

                // בדיקה אם קיימת תחנה עם stationName כמספר
                bool stationExists = listOfStation.Any(l => l.StationNumber.ToString() == stationName);
                if (!stationExists)
                {
                    throw new Exception($"The station with number '{stationName}' does not exist in the system.");
                }

                // הוספת הקובץ לרשימה המתאימה בתחנה
                if (!stationFiles.ContainsKey(stationName))
                {
                    stationFiles[stationName] = new List<string>();
                }
                stationFiles[stationName].Add(filePath);
            }

            return stationFiles;
        }

        public async Task CreateAsync(BlMeasuringStation measuringStation)
        {
            // בדיקת תקינות
            if (measuringStation == null)
                throw new ArgumentNullException(nameof(measuringStation));

            if (string.IsNullOrWhiteSpace(measuringStation.StationAddress))
                throw new ArgumentException("Station address cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(measuringStation.Town))
                throw new ArgumentException("Town cannot be null or empty.");

            if (measuringStation.StationNumber <= 0)
                throw new ArgumentException("Station number must be a positive integer.");

            // המרת BlMeasuringStation ל-MeasuringStation
            MeasuringStation entity = GetMeasuringStation(measuringStation);

            var existingStations = await dal.MeasuringStation.GetAsync();
            var existingStation = existingStations
                .FirstOrDefault(s => s.StationNumber == entity.StationNumber);

            if (existingStation != null)
            {
                // אם התחנה קיימת – נעביר לעדכון (BL UpdateAsync)
                await UpdateAsync(measuringStation);
            }
            else
            {
                // אחרת – יוצרים תחנה חדשה ב-DAL
                await dal.MeasuringStation.CreateAsync(entity);
            }
        }


        public async Task UpdateAsync(BlMeasuringStation measuringStation)
        {
            // בדיקת תקינות
            if (measuringStation == null)
                throw new ArgumentNullException(nameof(measuringStation));

            if (measuringStation.StationNumber <= 0)
                throw new ArgumentException("Station number must be a positive integer.");

            if (string.IsNullOrWhiteSpace(measuringStation.StationAddress))
                throw new ArgumentException("Station address cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(measuringStation.Town))
                throw new ArgumentException("Town cannot be null or empty.");

            // המרת BlMeasuringStation ל-MeasuringStation
            MeasuringStation entity = GetMeasuringStation(measuringStation);

            // עדכון ב-DAL בגרסה אסינכרונית
            await dal.MeasuringStation.UpdateAsync(entity);
        }

        // פונקציית עזר להמרת BL ל-DAL
        private MeasuringStation GetMeasuringStation(BlMeasuringStation measuringStation) =>
            new MeasuringStation
            {
                StationNumber = measuringStation.StationNumber,
                StationAddress = measuringStation.StationAddress,
                Town = measuringStation.Town,
                StationManager = measuringStation.StationManager
            };

        public async Task<List<BlMeasuringStation>> GetAsync()
        {
            string baseDir = AppContext.BaseDirectory;
            string dataDir = Path.Combine(baseDir, "Data");

            Dictionary<string, List<string>> classificationOfFileByStation =
                await ClassificationOfFilePathByStationNameAsync(dataDir);//סיווג הקבצים בתיקייה למילון בו המפתח הוא שם תחנה והערך זה רשימת הקבצים השייכים לאותה תחנה 

            Dictionary<string, List<BlMeasurement>> listOfMeasurementObjects = await
                BlMeasurementService.ListOfMeasurementObjectsForTheStationAsync(classificationOfFileByStation);//המרת הקבצים של כל תחנה לרשימת מדידות ושמירתם במילון

            // שליפת רשימת התחנות ב-DAL
            List<MeasuringStation> list = await dal.MeasuringStation.GetAsync();

            List<BlMeasuringStation> blMeasuringStation = new List<BlMeasuringStation>();

            foreach (var kvp in listOfMeasurementObjects)
            {
                string key = kvp.Key; // שם התחנה (כמספר בטקסט)
                List<BlMeasurement> values = kvp.Value; // מדידות של תחנה 

                // לחפש תחנה עם אותו מספר
                var measuringStation = list.FirstOrDefault(ms => ms.StationNumber == Convert.ToInt32(key));
                if (measuringStation == null)
                    throw new Exception($"The measurement file belongs to a station ({key}) that does not exist in the system.");

                var station = new BlMeasuringStation
                {
                    StationNumber = Convert.ToInt32(key),
                    StationAddress = measuringStation.StationAddress,
                    Town = measuringStation.Town,
                    StationManager = measuringStation.StationManager,
                    Measurements = values ?? new List<BlMeasurement>()
                };

                blMeasuringStation.Add(station);
            }
           
            list//מתוך רשימת התחנות נמצא את אלו שלא הוכנסו לרשימה עם המדידות המתאימות להם. ז"א אלו תחנות ללא מדידות כרגע- נאחסנם עם רשימה ריקה כברירת מחדל
            .Where(measuringStation => !blMeasuringStation.Any(bs => bs.StationNumber == measuringStation.StationNumber))
            .ToList()
            .ForEach(measuringStation => blMeasuringStation.Add(new BlMeasuringStation
            {
                StationNumber = measuringStation.StationNumber,
                StationAddress = measuringStation.StationAddress,
                Town = measuringStation.Town,
                StationManager = measuringStation.StationManager,
                Measurements = new List<BlMeasurement> { new BlMeasurement() } 
            }));
            return blMeasuringStation;
        }

    }
}
