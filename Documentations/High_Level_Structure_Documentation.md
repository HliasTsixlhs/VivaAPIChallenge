# High-Level Structure of Viva.Geo.API Project

## Overview

This document provides an overview of the high-level structure of the Viva.Geo.API project, including its controllers,
services, repositories, and various configurations.

## Design/Architectural Patterns has been used at the Viva.Geo.API

The project implements several design patterns for efficient and scalable application design:

- **Repository Pattern**: Utilizes Entity Framework Core for data management.
- **DTOs**: Employs Data Transfer Objects (DTOs) for request/response handling.
- **Controller/Service Layering**: Adopts a layered architecture for separation of concerns.
- **Decoupling**: Differentiates between Common, Core, and DataAccess layers for loose coupling.
- **Health Check Pattern**: Incorporates health checks, enhancing the observability of the application.

## Controllers

Within the `Viva.Geo.API` project, the following controllers are present:

- `ArrayProcessingController.cs`
- `CountriesController.cs`

## Services

The core logic of the application is handled by services located in the `Viva.Geo.API.Core` project:

- `ArrayProcessingService.cs`
- `BorderService.cs`
- `CountryBorderService.cs`

## Repositories

The data access layer is managed by repositories that interact with the `GeoContext` located in
the `Viva.Geo.API.DataAccess` project.

## Caching

Caching has been implemented for country data to improve performance and reduce database load.

## Core Registrations

The `program.cs` file sets up the project with support for:

- Health checks
- Exception handling
- API versioning
- Swagger for API documentation
- HSTS for security improvements in release versions
- Structured logging using Serilog

## Use Cases

The application includes several key use cases, each accompanied by images and detailed documentation:

1. **Finding the Second Biggest Integer of a Table**:
    - *Description*: This use case involves processing an array to find the second largest integer.

      ![ArrayProcessing1.PNG](Images%2FUseCases%2Fdevelopment%2FArrayProcessing1.PNG)
2. **Getting Borders and Data for All Countries**:
    - *Description*: This use case fetches data and border information for all countries.

      ![CountriesALL.PNG](Images%2FUseCases%2Fdevelopment%2FCountriesALL.PNG)
3. **Getting Borders and Data for a Specific Country by Name**:
    - *Description*: This use case focuses on retrieving border and detailed data for a country, identified by its name.

      ![CountryByName.PNG](Images%2FUseCases%2Fdevelopment%2FCountryByName.PNG)

## Conclusion

The Viva.Geo.API project is structured to provide a robust, scalable, and maintainable API. It adopts best practices and
patterns that are crucial for the development of modern web applications.
