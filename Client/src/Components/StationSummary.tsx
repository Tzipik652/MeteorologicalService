import React, { useEffect, useState } from 'react';
import { BlSummaryOfMeasurement } from '../models';
import { CircularProgress, Typography } from '@mui/material';

interface StationSummaryProps {
  stationNumber: number;
}

const StationSummary: React.FC<StationSummaryProps> = ({ stationNumber }) => {
  const [summary, setSummary] = useState<BlSummaryOfMeasurement | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const fetchSummary = async () => {
    setLoading(true);
    setError('');
    try {
      // שליפת כל הסיכומים
      const response = await fetch('https://localhost:7183/MeteorologicalService/GetSummaryOfMeasurement');
      if (!response.ok) {
        throw new Error('Failed to fetch summary');
      }
      const data: BlSummaryOfMeasurement[] = await response.json();

      const stationSummary = data.find(s => s.stationNumber === stationNumber);
      if (!stationSummary) {
        throw new Error(`No summary found for station #${stationNumber}`);
      }
      setSummary(stationSummary);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSummary();
  }, [stationNumber]);

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!summary) return null;

  return (
    <div style={{ marginTop: '1rem' }}>
      <Typography variant="h6">Station #{summary.stationNumber} Summary</Typography>
      <Typography>Max Temperature: {summary.maximumTemperature}</Typography>
      <Typography>Min Temperature: {summary.minimumTemperature}</Typography>
      <Typography>Max Rainfall: {summary.maximumRainfall}</Typography>
      <Typography>Min Rainfall: {summary.minimumRainfall}</Typography>
    </div>
  );
};

export default StationSummary;
