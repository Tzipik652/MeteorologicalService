using Bl.Api;
using Bl.Services;
using Dal;
using Dal.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Bl
{
    public class BlManager : IBL
    {
        public IBlMeasuringStation MeasuringStation { get; }
        public IBlSummaryOfMeasurement SummaryOfMeasurement { get; }
        public IBlMeasurement Measurement { get; }
        public BlManager()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IDal, DalManager>();
            services.AddScoped<IBlMeasuringStation, BlMeasuringStationService>();
            services.AddScoped<IBlSummaryOfMeasurement, BlSummaryOfMeasurementService>();
            services.AddScoped<IBlMeasurement, BlMeasurementService>();

            ServiceProvider provider = services.BuildServiceProvider();

            MeasuringStation = provider.GetRequiredService<IBlMeasuringStation>();
            SummaryOfMeasurement = provider.GetRequiredService<IBlSummaryOfMeasurement>();
            Measurement = provider.GetRequiredService<IBlMeasurement>();
        }


    }
}
