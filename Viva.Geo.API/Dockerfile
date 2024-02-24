# Base Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["Viva.Geo.API/Viva.Geo.API.csproj", "Viva.Geo.API/"]
COPY ["Common.HealthChecks/Common.HealthChecks.csproj", "Common.HealthChecks/"]
COPY ["Common.Logging.Serilog/Common.Logging.Serilog.csproj", "Common.Logging.Serilog/"]
COPY ["Common.Web/Common.Web.csproj", "Common.Web/"]
COPY ["Viva.Geo.API.Common/Viva.Geo.API.Common.csproj", "Viva.Geo.API.Common/"]
COPY ["Viva.Geo.API.Core/Viva.Geo.API.Core.csproj", "Viva.Geo.API.Core/"]
COPY ["Viva.Geo.API.DataAccess/Viva.Geo.API.DataAccess.csproj", "Viva.Geo.API.DataAccess/"]

RUN dotnet restore "Viva.Geo.API/Viva.Geo.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Viva.Geo.API"
RUN dotnet build "Viva.Geo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=${BUILD_CONFIGURATION}

RUN dotnet publish "Viva.Geo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage
FROM base AS final
# Set the environment variable
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=${BUILD_CONFIGURATION}

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Viva.Geo.API.dll"]
