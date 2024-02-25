# Unit Testing in Viva.Geo.API Project

![VivaGeoTestsRun.PNG](Images%2FTests%2FVivaGeoTestsRun.PNG)

## Importance of Testing

Testing is a crucial part of software development that ensures the correctness, functionality, and reliability of the
application. Different types of tests serve varied purposes in a project:

- **Unit Tests**: Verify the smallest testable parts of the application, typically methods and functions.
- **Integration Tests**: Check the interactions between different parts of the application, like modules or services.
- **Acceptance/Functional Tests**: Ensure the system meets the specified requirements and works as expected from an
  end-user perspective.
- **Contract Tests**: Validate the interactions between different services or systems.

In the Viva.Geo.API project, we have a comprehensive testing approach that includes unit tests and integration tests for
business logic, controllers, mappers, and for database interactions.

## Test Projects and Structure

### Unit tests

#### Viva.Geo.API.Core.Tests

This project contains unit tests for the services in the Viva.Geo.API.Core project:

- **ArrayProcessingServiceTests**
    - `FindSecondLargest_ShouldReturnsCorrectValue_WhenInputArrayIsValid`
    - `FindSecondLargest_ShouldThrowsInsufficientUniqueElementsException_WhenInputArrayIsNotValid`
- **BorderServiceTests**
    - Test cases for creating or updating borders with various cache and database scenarios.
- **CountryBorderServiceTests**
    - Test cases for associating countries and borders with cache scenarios.
- **CountryServiceTests**
    - Test cases for retrieving and saving countries, handling external API and deserialization failures.

#### Viva.Geo.API.Tests

This project focuses on testing mappers:

- **MapExternalCountryInfoToCountryTests**
    - `Mapper_ShouldMapEssentialFields_WhenOnlyKeyPropertiesAreProvided`
    - `Mapper_ShouldMapEssentialFields_WhenCompleteModelIsProvided`

### Integration tests

#### Common.Persistence.EFCore.Tests

Tests for the repository pattern, utilizing FluentAssertions and the `Microsoft.EntityFrameworkCore.InMemory` package:

- **BaseRepositoryTests**
    - Comprehensive tests for CRUD operations in the repository pattern.

#### Viva.Geo.API.IntegrationTests

Integration tests for the API endpoints, ensuring that the application's web layer functions correctly and interacts
properly with its services and data layer. These tests utilize `Viva.Geo.WebApplicationFactory` for creating a test
environment and make real HTTP requests to various endpoints. The primary testing tools include `HttpClient` for sending
requests and `FluentAssertions` for validating responses:

- **ArrayProcessingControllerTests**
    - Tests targeting the Array Processing controller's functionality. Specifically, it includes tests for endpoints
      that handle array manipulation operations, ensuring they return the correct data and status codes.

- **CountriesControllerTests**
    - Focuses on testing the Countries controller's endpoints. These tests verify the functionality of retrieving
      country-specific data, such as fetching details for a specific country and ensuring correct caching behavior of
      the endpoints.

Each test class in `Viva.Geo.API.IntegrationTests` aims to validate the reliability and correctness of the API's
response under various scenarios, simulating real-world usage as closely as possible. The tests ensure that the
endpoints handle requests as expected and return the correct data, following the specified business logic and
requirements.

## Conclusion

The structured approach to testing in the Viva.Geo.API project ensures robustness and reliability, paving the way for a
maintainable and scalable application. By covering a wide range of test scenarios, we ensure that each component
functions correctly individually and in conjunction with others.
