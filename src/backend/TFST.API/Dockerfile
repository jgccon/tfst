# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/TFST.API/TFST.API.csproj", "backend/TFST.API/"]
COPY ["backend/TFST.Modules.Users.Presentation/TFST.Modules.Users.Presentation.csproj", "backend/TFST.Modules.Users.Presentation/"]
COPY ["backend/TFST.Modules.Users.Application/TFST.Modules.Users.Application.csproj", "backend/TFST.Modules.Users.Application/"]
COPY ["backend/TFST.Modules.Users.Persistence/TFST.Modules.Users.Persistence.csproj", "backend/TFST.Modules.Users.Persistence/"]
COPY ["backend/TFST.Modules.Users.Domain/TFST.Modules.Users.Domain.csproj", "backend/TFST.Modules.Users.Domain/"]
COPY ["backend/TFST.SharedKernel.Domain/TFST.SharedKernel.Domain.csproj", "backend/TFST.SharedKernel.Domain/"]
COPY ["backend/TFST.SharedKernel.Configuration/TFST.SharedKernel.Configuration.csproj", "backend/TFST.SharedKernel.Configuration/"]
COPY ["backend/TFST.SharedKernel.Persistence/TFST.SharedKernel.Persistence.csproj", "backend/TFST.SharedKernel.Persistence/"]
COPY ["backend/TFST.SharedKernel.Presentation/TFST.SharedKernel.Presentation.csproj", "backend/TFST.SharedKernel.Presentation/"]
RUN dotnet restore "./backend/TFST.API/TFST.API.csproj"
COPY . .
WORKDIR "/src/backend/TFST.API"
RUN dotnet build "./TFST.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TFST.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TFST.API.dll"]