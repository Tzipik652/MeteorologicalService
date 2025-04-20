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
    public class MeasuringStationService : IMeasuringStation
    {

        MyContext _dataManager;
        public MeasuringStationService(MyContext? m)
        {
            _dataManager = m;
        }
        public async Task CreateAsync(MeasuringStation measuringStation)
        {
            if (measuringStation == null)
                throw new ArgumentNullException(nameof(measuringStation), "measuring station cannot be null.");

            if (measuringStation.StationNumber <= 0)
                throw new ArgumentException("Station number must be a positive integer.");

            if (string.IsNullOrWhiteSpace(measuringStation.StationAddress))
                throw new ArgumentException("Station address cannot be empty.");

            // ווידוא שאין תחנה עם אותו מזהה (באופן אסינכרוני)
            if (await _dataManager.MeasuringStations.AnyAsync(c => c.StationNumber == measuringStation.StationNumber))
                throw new Exception($"A station with station number {measuringStation.StationNumber} already exists.");

            // הוספה ושמירה (באופן אסינכרוני)
            await _dataManager.MeasuringStations.AddAsync(measuringStation);
            await _dataManager.SaveChangesAsync();
        }

        public async Task UpdateAsync(MeasuringStation measuringStation)
        {
            if (measuringStation == null)
                throw new ArgumentNullException(nameof(measuringStation), "measuring station cannot be null.");

            var measuringStationToUpdate = await _dataManager.MeasuringStations.FindAsync(measuringStation.StationNumber);
            if (measuringStationToUpdate == null)
                throw new Exception($"Measuring station with station number {measuringStation.StationNumber} does not exist.");

            if (string.IsNullOrWhiteSpace(measuringStation.StationAddress))
                throw new ArgumentException("Station address cannot be empty.");

            // עדכון השדות
            measuringStationToUpdate.StationAddress = measuringStation.StationAddress;
            measuringStationToUpdate.Town = measuringStation.Town;
            measuringStationToUpdate.StationManager = measuringStation.StationManager;

            // שמירה בגרסה אסינכרונית
            await _dataManager.SaveChangesAsync();
        }


        public async Task<List<MeasuringStation>> GetAsync()
        {
            try
            {
                return _dataManager.MeasuringStations.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
