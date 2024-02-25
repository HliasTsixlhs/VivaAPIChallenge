
# Vivia.com Code Challenge - Viva.Geo.API
<img src="Documentations/Images/viva_geo_api.jpg" alt="hermes.png" width="1100" height="650"/>

## Project Overview

This repository contains the code challenge for Vivia.com, designed to assess the capabilities and skills of job applicants in software development. The challenge focuses on various aspects of software engineering, including programming in C#, architecture, database integration, and debugging.

## Technology Stack

- **Framework**: .NET 7.0
- **IDE**: Rider or Visual Studio 
- **Database**: Microsoft SQL Server

## Included Projects

- **Common Projects**:
    - Common.HealthChecks
    - Common.Logging.Serilog
    - Common.MemoryCaching
    - Common.Persistence.EFCore
    - Common.Web

- **API Projects**:
    - Viva.Geo.API
    - Viva.Geo.API.Common
    - Viva.Geo.API.Core
    - Viva.Geo.API.DataAccess

- **Test Projects (xUnit)**:
    - Common.Persistence.EFCore.Tests
    - Viva.Geo.API.Core.Tests
    - Viva.Geo.API.Tests

## Installation and Setup


To get started with this project, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution file in Visual Studio or Rider.
3. Ensure that .NET 7.0 is installed and set as the target framework.
4. Configure the database connection strings as per your SQL Server setup.
5. Build the solution to restore any missing NuGet packages.
6. Migrate the database.
7. Run the application.

For a Comprehensive Step-by-Step Guide: [Installation Guide](Documentations/Installation_Guide.md)


## API Features and Architecture

### Core Solution

1. **Array Processing Controller**: A controller that returns the second largest integer from an array.
2. **Data Retrieval with Caching**: Controllers that retrieve data from the Rest Countries open API, illustrating the use of caching for performance improvement. This includes an endpoint to retrieve data by a country's name, demonstrating caching benefits for inner entities.
3. **Decoupled Architecture**: The API is divided into four projects for better separation of concerns:
  - **Viva.Geo.API**: Contains the controllers and serves as the web project.
  - **Viva.Geo.API.Core**: The heart of the project, containing business logic, services, and repositories.
  - **Viva.Geo.API.Common**: Holds shared resources like request/response DTOs, facilitating reuse across projects.
  - **Viva.Geo.API.DataAccess**: Manages the GeoContext, data access models, and migrations, separating the data access layer from the business layer.

4. **Unit Tests**: Comprehensive unit testing for core functionalities, covering arithmetic, array processing, system flow (caching, database fetching, exception handling).

### Supplementary Features

1. **Health Checks**: Implemented using `AspNetCore.HealthChecks` and `AspNetCore.HealthChecks.UI`. See [Common.HealthChecks Documentation](Documentations/Common_HealthChecks.md) for more details.
2. **Structured Logging**: Utilizes Serilog and its enrichers for semantic logging. For more information, refer to [Serilog Documentation](Documentations/SerilogDocumentation.md).
3. **Memory Caching**: A common library, `Common.MemoryCaching`, is used in the `Viva.Geo.API.Core` project.
4. **Repository Pattern**: Implemented in the `Common.Persistence.EFCore` project and thoroughly tested in `Common.Persistence.EFCore.Tests`. For more details, see [Repository Pattern Documentation](Documentations/RepositoryPatternDocumentation.md).
5. **Common.Web Project**: Includes a `CorrelationIdMiddleware` to demonstrate integration in a distributed system, aiding in client usage tracking. More information can be found in [Common.Web Documentation](Documentations/Common.WebDocumentation.md).
6. **API Versioning**: Supports backward compatibility through versioning, accomplished using the `Asp.Versioning.Mvc` package. This enables maintaining different versions of the API simultaneously. More information related to why we need this feature can be found [here](Documentations/GeneralNotes/Theory_BackwardsCompatibility.md).

## Contributions

While this project is primarily for assessment purposes, contributions or suggestions are welcome. Please refer to the contribution guidelines in the `CONTRIBUTING.md` file.

## License

This project is licensed under Viva.com ^ ^.


- [Usage Examples](Documentations/UsageExamples.md)
- [API Reference](Documentations/APIReference.md)