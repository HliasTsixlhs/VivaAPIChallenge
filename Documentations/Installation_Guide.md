# Installation and Setup Guide for Viva.Geo.API

## Getting Started

To set up the Viva.Geo.API project on your local machine, follow these steps:

1. **Clone the Repository**: Clone the project repository to your local machine.
2. **Open the Solution**: Open the solution file in Visual Studio or JetBrains Rider.
3. **Install .NET 7.0**: Ensure that .NET 7.0 is installed and set as the target framework.
4. **Database Configuration**:
    - **SQL Server Requirement**: SQL Server is required for database operations.
    - **Using Docker for SQL Server**:
        - Install Docker Desktop and set it to Linux containers
          mode [How To Install Docker Desktop with WSL2 Guide](HowToGuides/How_to_install_Docker_WSL2_Guide.md).
        - Pull a Docker image for SQL Server: `docker pull mcr.microsoft.com/mssql/server`
        - Run the SQL Server container with the following command:
          ```bash
          docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssword" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server
          ```

        - Use the following connection string in your application:
          `"ConnectionString": "Server=localhost,1433;Database=GeoDatabase;User ID=sa;Password=P@ssword;TrustServerCertificate=True;"`

5. **Build the Solution**: Build the solution to restore any missing NuGet packages.
6. **Database Migration**:
    - Navigate to the `Viva.Geo.API.DataAccess` project.
    - Run the EF Core database migration
      command: `dotnet ef database update`. [How_to_EFCore_Update_Guide.md](HowToGuides/How_to_EFCore_Update_Guide.md)
    - Alternatively, use the `EFMigrationScript.ps1` script in the root folder of the project. Refer
      to [How To Context Migration Guide](HowToGuides/How_To_ContextMigrationGuide.md) for more details.

7. **Running the Application**:
    - **Using an IDE**:
        - Open the project's solution file with Visual Studio or Rider.
        - Run the project.
    - **Alternatively Using Docker**:
        - Set up the network for Docker containers:
          ```
          docker network create vivaGeoNetwork
          docker network connect vivaGeoNetwork sqlserver
          docker network inspect vivaGeoNetwork
          ```
        - Ensure the connection string for Docker setup (at the appsettings.json file of the Viva.Geo.API)
          uses `Server=sqlserver,1433`.
        - Use the `docker_compose_control.ps1` script at the root folder of the project to manage the Docker setup.
          Command:

      ```bash
      .\docker_compose_control.ps1
      ```

## Docker Compose Configuration Notes

When setting up the Viva.Geo.API in Docker, ensure to follow these steps for network configuration to facilitate
communication between the SQL Server container and the Viva.Geo.API container.
