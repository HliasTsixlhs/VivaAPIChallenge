# Unit Testing in Viva.Geo.API Project
![VivaGeoTestsRun.PNG](Images%2FTests%2FVivaGeoTestsRun.PNG)
## Importance of Testing

Testing is a crucial part of software development that ensures the correctness, functionality, and reliability of the application. Different types of tests serve varied purposes in a project:

- **Unit Tests**: Verify the smallest testable parts of the application, typically methods and functions.
- **Integration Tests**: Check the interactions between different parts of the application, like modules or services.
- **Acceptance/Functional Tests**: Ensure the system meets the specified requirements and works as expected from an end-user perspective.
- **Contract Tests**: Validate the interactions between different services or systems.

In the Viva.Geo.API project, we have a comprehensive testing approach that includes unit tests for business logic and mappers, and integration tests for database interactions.

## Test Projects and Structure

### Viva.Geo.API.Core.Tests

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

### Viva.Geo.API.Tests

This project focuses on testing mappers:

- **MapExternalCountryInfoToCountryTests**
    - `Mapper_ShouldMapEssentialFields_WhenOnlyKeyPropertiesAreProvided`
    - `Mapper_ShouldMapEssentialFields_WhenCompleteModelIsProvided`

### Common.Persistence.EFCore.Tests

Tests for the repository pattern, utilizing FluentAssertions and the `Microsoft.EntityFrameworkCore.InMemory` package:

- **BaseRepositoryTests**
    - Comprehensive tests for CRUD operations in the repository pattern.

## Conclusion

The structured approach to testing in the Viva.Geo.API project ensures robustness and reliability, paving the way for a maintainable and scalable application. By covering a wide range of test scenarios, we ensure that each component functions correctly individually and in conjunction with others.
