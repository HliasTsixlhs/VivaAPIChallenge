# Vivia.com Code Challenge - Viva.Geo.API

<img src="Documentations/Images/viva_geo_api_2.jpg" alt="hermes.png" width="787" height="450"/>

## Project Overview

This repository presents the solution for the Vivia.com code challenge! The challenge encompasses a wide range of
software engineering aspects, including programming in C#, architecture, database integration, and debugging. This
solution aims to demonstrate a comprehensive approach to the challenge, showcasing best practices and efficient
problem-solving in software development.

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

### Core Features

1. **Array Processing Endpoint**: A controller that returns the second largest integer from an array.
2. **Countries Data Endpoints**: A controller that returns Data from the restCountries API that has been
   saved/cashed/fetched.
3. **Data Retrieval with Caching**: Services that retrieve data from the Rest Countries open API, illustrating the use
   of caching for performance improvement. This includes an endpoint to retrieve data by a country's name (not mandatory
   requirement of the challenge), demonstrating caching benefits for inner entities.
4. **Decoupled Architecture**: The API is divided into four projects for better separation of concerns:

- **Viva.Geo.API**: Contains the controllers and serves as the web project.
- **Viva.Geo.API.Core**: The heart of the project, containing business logic, services, and repositories.
- **Viva.Geo.API.Common**: Holds shared resources like request/response DTOs, facilitating reuse across projects.
- **Viva.Geo.API.DataAccess**: Manages the GeoContext, data access models, and migrations, separating the data access
  layer from the business layer.

5. **Unit Tests**: Comprehensive unit testing for core functionalities is implemented across the project. This includes
   testing for arithmetic operations, array processing, and system flow aspects such as caching, database interactions,
   and exception handling. For a detailed overview of our testing strategy, including the test cases and methodologies,
   refer to [Unit Testing Documentation](Documentations/Unit_Testing_Documentation.md).

### Supplementary Features

1. 🚑 **Health Checks**: Implemented using `AspNetCore.HealthChecks` and `AspNetCore.HealthChecks.UI`.
   See [Common.HealthChecks Docs](Documentations/Common_HealthChecks.md) for more details.
2. 📊 **Structured Logging**: Utilizes Serilog and its enrichers for semantic logging. For more information, refer
   to [Structured Logging Docs](Documentations/StructuredLogging.md).
3. 💾 **Memory Caching**: A common library, `Common.MemoryCaching`, is used in the `Viva.Geo.API.Core` project.
   See [Common.MemoryCaching Docs](Documentations/Common.MemoryCaching.md) for more details.
4. 🗃️ **Repository Pattern**: Implemented in the `Common.Persistence.EFCore` project and thoroughly tested
   in `Common.Persistence.EFCore.Tests`. For more details,
   see [Repository Pattern Docs](Documentations/RepositoryPatternDocumentation.md).
5. 🕸️ **Common.Web Project**: Includes a `CorrelationIdMiddleware` to demonstrate integration in a distributed system,
   aiding in client usage tracking. More information can be found
   in [Common.Web Docs](Documentations/Common_Web_Documentation.md).
6. 🔢 **API Versioning**: Supports backward compatibility through versioning, accomplished using the `Asp.Versioning.Mvc`
   package. This enables maintaining different versions of the API simultaneously. More information related to why we
   need this feature can be
   found [Backwards Compatibility Theory](Documentations/GeneralNotes/Theory_BackwardsCompatibility.md).
7. 📚 **API Documentation with Swagger**: Provides interactive documentation and testing capabilities using
   the `Swashbuckle.AspNetCore` package. The Swagger UI facilitates easy exploration and interaction with the API's
   endpoints. For a detailed guide on our Swagger implementation,
   see [Swagger Documentation](Documentations/Swagger_Documentation.md).
8. ❗ **Structured Error Handling with ProblemDetails**: The API leverages Microsoft's `ProblemDetails` feature for
   returning structured error responses, enhancing clarity and actionability. It ensures errors are universally
   understandable without exposing sensitive details in production. For more insights into our implementation and best
   practices, refer to [ProblemDetails Documentation](Documentations/ProblemDetails_Documentation.md).

## Project Structure Reference

For an in-depth overview of the Viva.Geo.API project's architecture, including its controllers, services, design
patterns and Use Cases, refer to
the [High-Level Structure Documentation](Documentations/High_Level_Structure_Documentation.md) located in
the `Documentations` folder. This document outlines the core components and configurations of the project, such as
health checks, exception handling, API versioning, Swagger integration, and HSTS, as well as key design patterns like
the Repository pattern, DTOs, and controller/service layering.

## Configuration Reference

For detailed information on the configuration settings of Viva.Geo.API, please refer to
the [Application Configuration](Documentations/Application_Configuration.md) located in the `Documentations` folder.
This document outlines the configurations for Serilog, HealthCheckUI, MemoryCacheOptions, DatabaseOptions, and other
important settings.

## Database Structure Reference

For a detailed understanding of the database architecture and table relationships of the Viva.Geo.API, refer to
the [Database Structure Documentation](Documentations/Database_Structure_Documentation.md) located in
the `Documentations` folder. This document provides a comprehensive overview of the tables, relationships, and design
considerations, including the rationale behind the many-to-many relationship implementation between the `Countries`
and `Borders` entities

## Project Checklist

- ✅ **Application Tested**: Comprehensive unit and integration tests implemented.
- ✅ **Application Decoupled**: Successfully achieved decoupling across Common/Core/DataAccess layers.
- ✅ **Health Checks Implemented**: Using `AspNetCore.HealthChecks` for system health monitoring.
- ✅ **Structured Logging**: Integrated with Serilog for enhanced logging.
- ✅ **Memory Caching**: Implemented with `Common.MemoryCaching`.
- ✅ **Repository Pattern**: Utilized in data access layer for efficient database interactions.
- ✅ **API Versioning**: Ensures backward compatibility with `Asp.Versioning.Mvc`.
- ✅ **Swagger Documentation**: Interactive API documentation using `Swashbuckle.AspNetCore`.
- ✅ **ProblemDetails for Error Handling**: Structured error responses following best practices.
- ✅ **Docker/Containerization Support**: Implemented with Dockerfile, docker-compose, and PowerShell scripts for
  container management.

## Note to Evaluators

This checklist is designed to provide a quick overview of the Viva.Geo.API's capabilities and features. Each item
represents a key aspect of the project, aligned with industry best practices and standards.

I would also like to take this opportunity to express my gratitude for the challenge presented to me. Working on this
project has not only refreshed my memory on various exciting aspects related to APIs but has also been a valuable
learning experience. I've gained a lot of new insights and knowledge throughout this process, and for that, I am truly
thankful. 😊

## Contributions

While this project is primarily for assessment purposes, contributions or suggestions are welcome ^_^.

## Future Considerations

As the Viva.Geo.API project evolves, there are several enhancements and additions that could further improve its
functionality and scalability:

1. **Hard Fetching Mechanism for Country Data**:
    - Implementing a mechanism to refetch country data from the Rest Countries API without relying on the database or
      cache is crucial. This can ensure the data remains up-to-date, especially regarding changes in country borders.
    - This could be achieved through a dedicated API endpoint, a background worker process, or a periodic task that
      fetches and stores new data.

2. **Scaling and Asynchronous Communication for Microservice Architecture**:
    - To scale the application and integrate it into a microservices ecosystem, support for asynchronous communication
      is essential.
    - Consider integrating worker support packages like Hangfire to set up asynchronous communication using message
      brokers like RabbitMQ or Azure Bus Service.
    - Implementing the transactional outbox/inbox pattern and embracing an Event-Driven Architecture (EDA) can further
      enhance the system's reliability and scalability in processing asynchronous events and messages.
    - This change will also influence the API's design, leaning towards a pragmatic REST API style. The API could then
      support both CRUD operations and long-running tasks, with endpoints designed around verbs and nouns to accommodate
      different types of requests.

These future considerations aim to enhance the application's data accuracy, scalability, and interoperability within a
distributed system, aligning with modern software architecture practices.

## License

💳 This project is licensed under Viva.com ^_^.