# Build stage
FROM node:22-slim AS builder
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build -- --project=tfst-app --configuration=production
# The build artifacts will be stored in the dist/tfst-app directory

# Production Nginx server
FROM nginx:alpine
# Copy the built application from the builder stage to the Nginx HTML directory
COPY --from=builder /app/dist/tfst-app/browser /usr/share/nginx/html
# Copy the Nginx configuration file
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

