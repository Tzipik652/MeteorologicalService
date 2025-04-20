using Bl.Api;
using Bl.Models;
using Bl;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MeteorologicalServiceController : ControllerBase
    {
        private readonly IBL _bl;

        public MeteorologicalServiceController(IBL bl)
        {
            _bl = bl;
        }

        //בניית רשימת תחנות - טעינת הנתונים מהדטה בייס
        //פונקציה שעוברת על כל הקבצים בתקיה ומוסיפה לכל תחנה את נתוני המדידות שלה
        [HttpGet("DataLoading")]
        public async Task<List<BlMeasuringStation>> DataLoading()
        {

            var stations = await _bl.MeasuringStation.GetAsync() as List<BlMeasuringStation>;

            return stations ?? new List<BlMeasuringStation>(); // מחזיר רשימה ריקה אם אין נתונים
        }

        //שליפת נתונים:
        //a.להציג רשימת תחנות
        //b.לקבל תחנה ולהציג רשימת מדידות

        [HttpGet("DataRetrieval")]
        public async Task<IActionResult> DataRetrieval()
        {

            try
            {
                var stations = await DataLoading();
                return Ok(stations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("RetrievingAListOfMeasurementsForAStation/{stationNumber}")]
        public async Task<IActionResult> RetrievingAListOfMeasurementsForAStation(int stationNumber)
        {
            try
            {
                var stations = await DataLoading();
                var station = stations.FirstOrDefault(s => s.StationNumber == stationNumber);

                if (station == null)
                    return NotFound(new { message = $"Station with number {stationNumber} not found." });

                return Ok(station.Measurements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("GetSummaryOfMeasurement")]
        public async Task<IActionResult> GetSummaryOfMeasurement()
        {
            try
            {
                await ExportData();
                var summaries = await _bl.SummaryOfMeasurement.GetAsync();
                return Ok(summaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        //יצוא נתונים:
        //לשמור בטבלה את הנתונים הבאים:
        //מספר תחנה, טמפרטורה מקסימלית, טמפ מינימלית, כמות גשמים מקסימלית, כמות גשמים מינימלית
        [HttpPost("ExportData")]
        public async Task<IActionResult> ExportData()
        {
            try
            {
                var stations = await DataLoading();

                foreach (var station in stations)
                { 
                    await _bl.SummaryOfMeasurement.CreateAsync(
                        station.StationNumber.ToString(),
                        station.Measurements
                    );
                  
                }

                return Ok(new { message = "Data export completed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("CreateStation")]
        public async Task<IActionResult> CreateStation([FromBody] BlMeasuringStation newStation)
        {
            try
            {
                await _bl.MeasuringStation.CreateAsync(newStation);
                return Ok();
            }
          
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    

    }
}
