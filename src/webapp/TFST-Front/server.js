const express = require('express');
const path = require('path');

const app = express();
const PORT = process.env.PORT || 8080;

// Log to indicate the server is starting
console.log(`Starting server on port ${PORT}...`);

// Middleware to log every incoming request
app.use((req, res, next) => {
  console.log(`[${new Date().toISOString()}] ${req.method} ${req.url}`);
  next();
});

// Log to confirm the directory serving static files
const staticPath = path.join(__dirname, 'browser'); 
console.log(`Serving static files from: ${staticPath}`);
app.use(express.static(staticPath));

// Log when a route is not found and redirect to index.html
app.get('/*', (req, res) => {
  console.log(`Route not found: ${req.url}. Redirecting to index.html`);
  res.sendFile(path.join(staticPath, 'index.html'), (err) => {
    if (err) {
      console.error(`Error serving index.html: ${err.message}`);
      res.status(500).send('Internal server error');
    }
  });
});

// Log to confirm the server is listening correctly
app.listen(PORT, () => {
  console.log(`[${new Date().toISOString()}] Server running on port ${PORT}`);
});

// Handle uncaught exceptions
process.on('uncaughtException', (err) => {
  console.error(`Uncaught exception: ${err.message}`);
  process.exit(1); // Exit the process with an error
});

// Handle unhandled promise rejections
process.on('unhandledRejection', (reason, promise) => {
  console.error(`Unhandled promise rejection: ${reason}`);
});
