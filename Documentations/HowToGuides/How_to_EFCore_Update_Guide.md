## Entity Framework Core Database Update Guide for Viva.Geo.API Project

1. **Install Entity Framework Core Tools:**

    - Install the EF Core Tools globally using PowerShell:
      ```powershell
      dotnet tool install --global dotnet-ef
      ```
    - Update to the latest version if already installed:
      ```powershell
      dotnet tool update --global dotnet-ef
      ```

2. **Navigate to the Viva.Geo.API Project Directory:**

    - Use the `cd` command in PowerShell to navigate to your project directory.
    - Example:
      ```powershell
      cd path_to\Viva.Geo.API
      ```

3. **Run the EF Core Database Migration Command:**

    - In the project directory, run the following command:
      ```powershell
      dotnet ef database update --context GeoContext
      ```
    - This applies the latest migrations to the database using the `GeoContext` context.