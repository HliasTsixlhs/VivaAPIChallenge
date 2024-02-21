
# Migrating GeoContext in a Multi-Project Solution using Entity Framework Core (EF Core)

This guide provides instructions on performing database migrations in a multi-project solution where the `GeoContext` DbContext is in a different project than the startup project (which contains `program.cs`).

## Prerequisites

- .NET SDK installed.
- Entity Framework Core CLI tools installed. Install globally using:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Steps for Migrating FinancialAnalyticsContext

### 1. Add Required NuGet Packages

Ensure the project containing your `GeoContext` has the following NuGet packages:

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- Database provider package (e.g., `Microsoft.EntityFrameworkCore.SqlServer`)

Use the .NET CLI to add these packages:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### 2. Define Your DbContext

Your `GeoAnalyticsContext` should be properly defined in the data access project with `DbSet` properties for your entities.

### 3. Create a Migration

Run the EF Core `migrations add` command from the root directory of your solution or any directory that can access both projects. Specify the project paths using `--project` and `--startup-project` options, and also specify the DbContext using `--context`:

```bash
dotnet ef migrations add InitialCreate --project Path/To/DataAccessProject.csproj --startup-project Path/To/WebAPIProject.csproj --context GeoContext
```

Replace `Path/To/DataAccessProject.csproj` and `Path/To/WebAPIProject.csproj` with your actual project paths and `InitialCreate` with your desired migration name.

### 4. Update the Database

Apply the migration to update your database schema:

```bash
dotnet ef database update --project Path/To/DataAccessProject.csproj --startup-project Path/To/WebAPIProject.csproj --context GeoContext
```


## Use the EFMigrationScript.ps1 at the root of the project

For the Viva.Geo.API.DataAccess project:
```shell
.\EFMigrationScript.ps1 -migrationName YourMigrationName -projectPath .\Viva.Geo.API.DataAccess\Viva.Geo.API.DataAccess.csproj -startupProjectPath .\Viva.Geo.API\Viva.Geo.API.csproj -dbContextName GeoContext
```


## Notes

- Make sure the `GeoContext` is correctly configured with the database provider and connection string.
- Be cautious with migrations in production environments. Use controlled deployment processes.
- Track migration files in your version control system.
