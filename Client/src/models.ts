export interface BlMeasurement {
    date: string; // or Date
    measurementTime: string; // or TimeSpan (sent as "HH:MM:SS") 
    temperature: number;
    amountOfRain: number;
    windSpeed: number;
  }
  
  export interface BlMeasuringStation {
    stationNumber: number;
    stationAddress: string;
    town: string;
    stationManager: string;
    measurements: BlMeasurement[];
  }
  
  export interface BlSummaryOfMeasurement {
    stationNumber: number;
    maximumTemperature: number;
    minimumTemperature: number;
    maximumRainfall: number;
    minimumRainfall: number;
  }
  