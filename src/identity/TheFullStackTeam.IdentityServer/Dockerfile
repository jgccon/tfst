# Stage 1: Build the Identity Server
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files for dependency restoration
COPY TheFullStackTeam.IdentityServer/*.csproj TheFullStackTeam.IdentityServer/
COPY TheFullStackTeam.IdentityServer.Domain/*.csproj TheFullStackTeam.IdentityServer.Domain/

# Restore dependencies
RUN dotnet restore "TheFullStackTeam.IdentityServer/TheFullStackTeam.IdentityServer.csproj"

# Copy the entire source and build the application
COPY . .
WORKDIR "/src/TheFullStackTeam.IdentityServer"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish the Identity Server
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Create a lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy compiled binaries from the build stage
COPY --from=publish /app/publish .

# Expose necessary ports for HTTP and HTTPS
EXPOSE 7200
EXPOSE 7201

# Set the ASP.NET Core URLs
ENV ASPNETCORE_URLS="http://+:7200;https://+:7201"

# Start the Identity Server application
ENTRYPOINT ["dotnet", "TheFullStackTeam.IdentityServer.dll"]
