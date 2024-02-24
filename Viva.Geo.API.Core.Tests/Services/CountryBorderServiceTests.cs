using Moq;
using Viva.Geo.API.Core.Services;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Logging;
using Common.Logging.Serilog.Factories.Abstractions;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Tests.Services;

public class CountryBorderServiceTests
{
    private readonly Mock<ICountryBorderRepository> _mockCountryBorderRepository;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly CountryBorderService _countryBorderService;

    public CountryBorderServiceTests()
    {
        var mockLogger = new Mock<ILogger<CountryBorderService>>();
        var mockEventIdFactory = new Mock<IEventIdFactory>();
        _mockCountryBorderRepository = new Mock<ICountryBorderRepository>();
        _mockCacheService = new Mock<IMemoryCacheService>();

        _countryBorderService = new CountryBorderService(
            _mockCountryBorderRepository.Object,
            _mockCacheService.Object,
            mockLogger.Object,
            mockEventIdFactory.Object);
    }


    /// <summary>
    /// Tests whether AssociateCountryAndBorderAsync correctly uses cached data for a country-border relationship when available.
    /// </summary>
    /// <remarks>
    /// This test checks if the service efficiently retrieves the existing country-border association from the cache 
    /// instead of querying the repository again, ensuring optimal performance by avoiding unnecessary database access. 
    /// It confirms that the service properly utilizes caching mechanisms to manage country-border associations.
    /// </remarks>
    [Fact]
    public async Task AssociateCountryAndBorderAsync_ShouldReturnCachedCountries_WhenCacheHit()
    {
        // Arrange
        const int countryId = 1;
        const int borderId = 2;
        var cacheKey = $"countryBorder_{countryId}_{borderId}";
        var cachedCountryBorder = new CountryBorder {CountryId = countryId, BorderId = borderId};

        _mockCacheService.Setup(s => s.Get<CountryBorder>(cacheKey)).Returns(cachedCountryBorder);
        _mockCountryBorderRepository.Setup(r => r.GetAsync(countryId, borderId, It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        await _countryBorderService.AssociateCountryAndBorderAsync(countryId, borderId, CancellationToken.None);

        // Assert
        _mockCountryBorderRepository.Verify(
            r => r.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    /// <summary>
    /// Tests whether AssociateCountryAndBorderAsync effectively handles the case where a country-border relationship
    /// is not found in the cache but exists in the database.
    /// </summary>
    /// <remarks>
    /// This test ensures that when the cache misses, the service successfully queries the repository for an existing
    /// country-border relationship. It verifies that if the association is already present in the database, the service
    /// does not create a duplicate entry but instead updates the cache with the existing association. This scenario is
    /// critical for maintaining data integrity and avoiding redundant data storage.
    /// </remarks>
    [Fact]
    public async Task
        AssociateCountryAndBorderAsync_ShouldHandleExistingAssociationRepository_WhenCacheMissAndDatabaseHit()
    {
        // Arrange
        const int countryId = 1;
        const int borderId = 2;
        var cacheKey = $"countryBorder_{countryId}_{borderId}";
        var existingCountryBorder = new CountryBorder {CountryId = countryId, BorderId = borderId};

        // var mockCacheService = new Mock<IMemoryCacheService>();
        _mockCacheService.Setup(s => s.Get<CountryBorder>(cacheKey)).Returns((CountryBorder) null);

        _mockCountryBorderRepository.Setup(r => r.GetAsync(countryId, borderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCountryBorder);
        _mockCountryBorderRepository.Setup(r => r.CreateAsync(It.IsAny<CountryBorder>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCountryBorder);
        _mockCacheService.Setup(s => s.Set(cacheKey, existingCountryBorder, It.IsAny<TimeSpan>()));

        // Act
        await _countryBorderService.AssociateCountryAndBorderAsync(countryId, borderId, CancellationToken.None);

        // Assert
        _mockCacheService.Verify(s => s.Set(cacheKey, existingCountryBorder, It.IsAny<TimeSpan>()), Times.AtLeastOnce);
        _mockCountryBorderRepository.Verify(
            r => r.CreateAsync(It.IsAny<CountryBorder>(), It.IsAny<CancellationToken>()), Times.Never());
    }
}