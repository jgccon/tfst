# ─────── Stage 1: Build ───────
FROM node:22-alpine AS builder

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build


# ─────── Stage 2: Serve with NGINX ───────
FROM nginx:alpine

# Copy the built application from the builder stage to the Nginx HTML directory
COPY --from=builder /app/build /usr/share/nginx/html
# Copy the Nginx configuration file
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
