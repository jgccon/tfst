# Use official Node.js LTS image
FROM node:22-alpine

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the Docusaurus site (if needed)
RUN npm run build

# Expose the port Docusaurus runs on
EXPOSE 3000

# Start the Docusaurus server
CMD ["npm", "run", "serve"]