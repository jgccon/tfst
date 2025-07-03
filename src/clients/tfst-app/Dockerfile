# Use official Node.js LTS image
FROM node:22-slim

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

EXPOSE 4200

CMD ["npx", "ng", "serve", "--host", "0.0.0.0"]
