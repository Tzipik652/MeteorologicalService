import React from 'react';
import { Container, Typography, AppBar, Toolbar, Box } from '@mui/material';
import StationsList from './Components/StationsList';

function App() {
  return (
      <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
  <AppBar position="static">
    <Toolbar>
      <Typography variant="h6">Meteorological Service</Typography>
    </Toolbar>
  </AppBar>

  <Container sx={{ flexGrow: 1, marginTop: '2rem' }}>
    <StationsList />
  </Container>

  <Box sx={{ flexShrink: 0, textAlign: 'center', padding: 2, background: '#f5f5f5', borderTop: '1px solid #ddd' }}>
    <Typography variant="body2" color="textSecondary">
      All rights reserved &copy; 2025 | Developed by Tzipi Kroizer and Tehila Alkoby | Meteorological Service
    </Typography>
  </Box>
</Box>

  );
}

export default App;
