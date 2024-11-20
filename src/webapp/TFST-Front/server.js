const express = require('express');
const path = require('path');

const app = express();

// Serve static files from the browser folder
const browserPath = path.join(__dirname, 'browser');
app.use(express.static(browserPath));

// Redirect all other routes to index.html
app.get('/*', (req, res) => {
  res.sendFile(path.join(browserPath, 'index.html'));
});

// Start the server
const PORT = process.env.PORT || 8080;
app.listen(PORT, () => {
  console.log(`Node server is running on port ${PORT}`);
});
