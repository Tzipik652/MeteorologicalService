using Dal.Api;
using Dal.Models;
using Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
namespace Dal;

public class DalManager : IDal
{
    
    public IMeasuringStation MeasuringStation { get; }
    public ISummaryOfMeasurement SummaryOfMeasurement { get; }
    public IMeasurement Measurement { get; }
    public DalManager()
    {

        ServiceCollection serCollection = new ServiceCollection();
        // הגדרת אוביקט מסוג כל מחלקה והכנסתו לאוסף השרותים
        serCollection.AddSingleton<MyContext>();
        serCollection.AddScoped<IMeasuringStation, MeasuringStationService>();
        serCollection.AddScoped<ISummaryOfMeasurement, SummaryOfMeasurementService>();
        serCollection.AddScoped<IMeasurement, MeasurementService>();

        serCollection.AddDbContext<MyContext>(options =>
            options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\tzipi\\Desktop\\לימודים\\#c\\הגשת שב\\MeteorologicalService\\ProjectSummary\\station.mdf\";Integrated Security=True;Connect Timeout=30")); 

        // הגדרת ספק מחלקות שרות
        ServiceProvider p = serCollection.BuildServiceProvider();


        MeasuringStation = p.GetRequiredService<IMeasuringStation>();
        SummaryOfMeasurement = p.GetRequiredService<ISummaryOfMeasurement>();
        Measurement = p.GetRequiredService<IMeasurement>();

    }
}

