using Bl;
using Bl.Api;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Bl.Models;
using System.Collections.Generic;
namespace ProjectSummary
{
    internal class Program
    {
        ////בניית רשימת תחנות - טעינת הנתונים מהדטה בייס
        ////פונקציה שעוברת על כל הקבצים בתקיה ומוסיפה לכל תחנה את נתוני המדידות שלה
        //public static List<BlMeasuringStation> DataLoading(IBL bl)
        //{
        //    return bl.MeasuringStation.Get();
        //}
        ////שליפת נתונים:
        ////a.להציג רשימת תחנות
        ////b.לקבל תחנה ולהציג רשימת מדידות
        //public static void DataRetrieval(IBL bl)
        //{
        //    DataLoading(bl).ForEach(s => Console.WriteLine(
        //        $"Station Number: \x1b[38;2;144;238;144m{s.StationNumber}\x1b[0m: "
        //        + " Station Address: " + s.StationAddress + "  Town: " + s.Town 
        //        + " Station Manager: " + s.StationManager));
        //}
        //public static void RetrievingAListOfMeasurementsForAStation(IBL bl ,int stationNumber)
        //{
        //    Console.WriteLine($"\nStation Number: \x1b[38;2;144;238;144m{stationNumber}\x1b[0m");
        //    var listMeasurement = DataLoading(bl)
        //                          .Where(s => s.StationNumber == stationNumber)
        //                           .Select(s => s.Measurements).FirstOrDefault();
        //    listMeasurement.ForEach(m =>
        //    Console.WriteLine($"\u001b[38;2;144;238;144m Date:\u001b[0m {m.date}"
        //    + "\u001b[38;2;144;238;144m Measurement Time: \x1b[0m" + m.measurementTime + "\t\u001b[38;2;144;238;144m  Temperature: \x1b[0m"
        //    + m.temperature + "\t\u001b[38;2;144;238;144m Amount Of Rain: \x1b[0m" + m.amountOfRain + "\t\u001b[38;2;144;238;144m Wind Speed: \x1b[0m"
        //    + m.windSpeed)
        //    );
        //}
        ////יצוא נתונים:
        ////לשמור בטבלה את הנתונים הבאים:
        ////מספר תחנה, טמפרטורה מקסימלית, טמפ מינימלית, כמות גשמים מקסימלית, כמות גשמים מינימלית
        //public static void DataExport(IBL bl)
        //{
        //    List<BlMeasuringStation> blMeasuringStations = DataLoading(bl);
        //    //לנסות ליצור אובייקט סיכום מדידה חדש ואם הוא כבר קיים הוא רק יעדכן
        //    foreach(BlMeasuringStation m in blMeasuringStations)
        //    {
        //        try
        //        {
        //            bl.SummaryOfMeasurement.Create(m.StationNumber.ToString(), m.Measurements);
        //        }
        //        catch (Exception e) {
        //            if (e.Message.Contains("You have already created measurement summary data for station"))
        //            {
        //                bl.SummaryOfMeasurement.Update(m.StationNumber.ToString(), m.Measurements);
        //            }
        //            else
        //            {
        //                throw e;
        //            }
        //        }

        //    }
        //}
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("\u001b[38;2;100;149;255m Measuring Station \u001b[0m ");


        //    IBL bl = new BlManager();
        //    try
        //    {
        //        DataRetrieval(bl);
        //        RetrievingAListOfMeasurementsForAStation(bl, 123);

        //        DataExport(bl);
        //    }
        //    catch (Exception ex) { Console.WriteLine($"\x1b[31m{ex.Message}\x1b[0m"); }
        //}
        static void Main(string[] args) 
        {
            Console.WriteLine();
        }
    }
}
