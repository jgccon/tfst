const express = require('express');
const path = require('path');

const app = express();

// Serve static files from the Angular app
const appPath = path.join(__dirname, 'dist/tfst-front');
app.use(express.static(appPath));

// Handle all routes with the Angular app
app.get('*', (req, res) => {
  res.sendFile(path.join(appPath, 'index.html'));
});

// Start the server on the port Azure expects
const port = process.env.PORT || 8080;
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
