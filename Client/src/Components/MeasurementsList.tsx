import React from 'react';
import { BlMeasurement } from '../models';
import { Table, TableHead, TableRow, TableCell, TableBody, Typography } from '@mui/material';
import dayjs from 'dayjs';

interface MeasurementsListProps {
  measurements: BlMeasurement[];
}

const MeasurementsList: React.FC<MeasurementsListProps> = ({ measurements }) => {
  if (measurements.length === 0) {
    return <Typography>No measurements found.</Typography>;
  }

  return (
    <Table style={{ marginTop: '1rem' }}>
      <TableHead>
        <TableRow>
          <TableCell>Date</TableCell>
          <TableCell>Time</TableCell>
          <TableCell>Temperature</TableCell>
          <TableCell>Rainfall</TableCell>
          <TableCell>Wind Speed</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {measurements.map((m, index) => (
          <TableRow key={index}>
            <TableCell>{dayjs(m.date).format("DD/MM/YYYY")}</TableCell>
            <TableCell>{m.measurementTime}</TableCell>
            <TableCell>{m.temperature}</TableCell>
            <TableCell>{m.amountOfRain}</TableCell>
            <TableCell>{m.windSpeed}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
};

export default MeasurementsList;
