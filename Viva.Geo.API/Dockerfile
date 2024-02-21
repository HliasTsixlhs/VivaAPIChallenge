FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Viva.Geo.API/Viva.Geo.API.csproj", "Viva.Geo.API/"]
RUN dotnet restore "Viva.Geo.API/Viva.Geo.API.csproj"
COPY . .
WORKDIR "/src/Viva.Geo.API"
RUN dotnet build "Viva.Geo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Viva.Geo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Viva.Geo.API.dll"]
