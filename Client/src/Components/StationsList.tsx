import React, { useEffect, useState } from 'react';
import {
  List, ListItem, ListItemText, Button, CircularProgress,
  Typography, TextField, Dialog, DialogTitle, DialogContent, IconButton
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { BlMeasuringStation } from '../models';
import MeasurementsList from './MeasurementsList';
import StationSummary from './StationSummary';

const StationsList: React.FC = () => {
  const [stations, setStations] = useState<BlMeasuringStation[]>([]);
  const [filteredStations, setFilteredStations] = useState<BlMeasuringStation[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [selectedStation, setSelectedStation] = useState<BlMeasuringStation | null>(null);
  const [showMeasurements, setShowMeasurements] = useState(false);
  const [showSummary, setShowSummary] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [savedSearchTerm, setSavedSearchTerm] = useState(''); // משתנה שישמור את הערך המקורי של החיפוש

  const fetchStations = async () => {
    setLoading(true);
    setError('');
    try {
      const response = await fetch('https://localhost:7183/MeteorologicalService/DataRetrieval');
      if (!response.ok) {
        throw new Error('Failed to fetch stations');
      }
      const data: BlMeasuringStation[] = await response.json();
      setStations(data);
      setFilteredStations(data);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchStations();
  }, []);

  const handleSelectStation = (station: BlMeasuringStation) => {
    setSavedSearchTerm(searchTerm); // נשמור את ערך החיפוש
    setSearchTerm(''); // ננקה אותו כדי להסיר את ההדגשה
    setSelectedStation(station);
    setShowMeasurements(false);
    setShowSummary(false);
  };

  const handleCloseDialog = () => {
    setSearchTerm(savedSearchTerm); // נחזיר את החיפוש לאחר סגירת התיבה
    setSelectedStation(null);
  };

  const handleShowMeasurements = () => {
    setShowMeasurements(true);
    setShowSummary(false);
  };

  const handleShowSummary = () => {
    setShowSummary(true);
    setShowMeasurements(false);
  };

  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredStations(stations);
    } else {
      const lowerSearchTerm = searchTerm.toLowerCase();
      const isNumber = !isNaN(Number(searchTerm));

      setFilteredStations(
        stations.filter((station) =>
          station.town.toLowerCase().includes(lowerSearchTerm) ||
          station.stationAddress.toLowerCase().includes(lowerSearchTerm) ||
          station.stationManager.toLowerCase().includes(lowerSearchTerm) ||
          (isNumber && station.stationNumber.toString().includes(searchTerm))
        )
      );
    }
  }, [searchTerm, stations]);

  // פונקציה להדגשת החיפוש
  const highlightText = (text: string, highlight: string) => {
    if (!highlight.trim()) return text;

    const regex = new RegExp(`(${highlight})`, 'gi');
    return text.replace(regex, `<span style="background-color: yellow;">$1</span>`);
  };

  return (
    <div>
      <Typography variant="h5" gutterBottom>
        Stations List
      </Typography>

      {loading && <CircularProgress />}
      {error && <Typography color="error">{error}</Typography>}

      <TextField
        label="Type here to search..."
        variant="outlined"
        fullWidth
        margin="normal"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />

      <List>
        {filteredStations.map((station) => (
          <ListItem key={station.stationNumber}>
            <ListItemText
              primary={
                <span dangerouslySetInnerHTML={{
                  __html: `Station #${highlightText(station.stationNumber.toString(), searchTerm)} - ${highlightText(station.town, searchTerm)}`
                }} />
              }
              secondary={
                <span dangerouslySetInnerHTML={{
                  __html: `Address: ${highlightText(station.stationAddress, searchTerm)}, Manager: ${highlightText(station.stationManager, searchTerm)}`
                }} />
              }
            />
            <Button variant="outlined" onClick={() => handleSelectStation(station)}>
              To view station details
            </Button>
          </ListItem>
        ))}
      </List>

      <Dialog open={!!selectedStation} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        {selectedStation && (
          <>
            <DialogTitle>
              Station details #{selectedStation.stationNumber}
              <IconButton onClick={handleCloseDialog} style={{ position: 'absolute', right: 10, top: 10 }}>
                <CloseIcon />
              </IconButton>
            </DialogTitle>
            <DialogContent style={{ maxHeight: '70vh', overflowY: 'auto' }}>
              <Typography variant="h6">Station Information</Typography>
              <Typography>Town: {selectedStation.town}</Typography>
              <Typography>Address: {selectedStation.stationAddress}</Typography>
              <Typography>Manager: {selectedStation.stationManager}</Typography>

              <Button variant="contained" style={{ marginRight: '1rem', marginTop: '1rem' }} onClick={handleShowMeasurements}>
                View Measurements
              </Button>
              <Button variant="contained" style={{ marginTop: '1rem' }} onClick={handleShowSummary}>
                View Summary
              </Button>

              {showMeasurements && <MeasurementsList measurements={selectedStation.measurements} />}
              {showSummary && <StationSummary stationNumber={selectedStation.stationNumber} />}
            </DialogContent>
          </>
        )}
      </Dialog>
    </div>
  );
};

export default StationsList;
