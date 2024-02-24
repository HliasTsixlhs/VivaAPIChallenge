using System.Net;
using System.Text.Json;
using AutoMapper;
using Common.Logging.Serilog.Factories.Abstractions;
using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Viva.Geo.API.Common.Dtos.Borders.Responses;
using Viva.Geo.API.Common.Dtos.Countries.Responses;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.Core.Models;
using Viva.Geo.API.Core.Services;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Tests.Services;

public class CountryServiceTests
{
    private readonly Mock<ICountryRepository> _mockCountryRepository;
    private readonly Mock<IBorderService> _mockBorderService;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly CountryService _countryService;

    public CountryServiceTests()
    {
        var mockCountryBorderService = new Mock<ICountryBorderService>();
        var mockLogger = new Mock<ILogger<CountryService>>();
        var mockEventIdFactory = new Mock<IEventIdFactory>();
        _mockCountryRepository = new Mock<ICountryRepository>();
        _mockBorderService = new Mock<IBorderService>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();


        _countryService = new CountryService(
            _mockCountryRepository.Object,
            mockCountryBorderService.Object,
            _mockBorderService.Object,
            _mockCacheService.Object,
            _mockMapper.Object,
            _mockHttpClientFactory.Object,
            mockLogger.Object,
            mockEventIdFactory.Object);
    }

    /// <summary>
    /// Tests whether RetrieveAndSaveCountriesAsync correctly returns cached countries when they are available in the cache.
    /// </summary>
    /// <remarks>
    /// This test simulates a scenario where the requested countries are already stored in the cache.
    /// It verifies that the method returns the cached countries without making a call to the external country repository.
    /// </remarks>
    [Fact]
    public async Task RetrieveAndSaveCountriesAsync_ShouldReturnCachedCountries_WhenCacheHit()
    {
        // Arrange
        const string cacheKey = "All_Countries";
        var cachedCountries = new List<CountryDto>
        {
            new() {CommonName = "Country1", Borders = new List<string> {"Border1", "Border2"}},
            new() {CommonName = "Country2", Borders = new List<string> {"Border3", "Border4"}}
        };

        _mockCacheService.Setup(s => s.Get<IEnumerable<CountryDto>>(cacheKey)).Returns(cachedCountries);

        // Act
        var result = await _countryService.RetrieveAndSaveCountriesAsync(CancellationToken.None);

        // Assert
        Assert.Equal(cachedCountries, result);
        _mockCacheService.Verify(s => s.Get<IEnumerable<CountryDto>>(cacheKey), Times.Once);
        _mockCountryRepository.Verify(r => r.GetCountryByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Tests whether RetrieveAndSaveCountriesAsync correctly retrieves and returns a country from the database
    /// when the requested country data is not available in the cache, but exists in the database.
    /// </summary>
    /// <remarks>
    /// This test ensures that the method properly queries the country repository when the cache misses,
    /// confirming that it correctly handles the scenario where the requested country data needs to be fetched
    /// from an external API and is already stored in the database. It verifies the integration of different
    /// components (cache, external API, and repository) in retrieving country data.
    /// </remarks>
    [Fact]
    public async Task
        RetrieveAndSaveCountriesAsync_ShouldReturnExistingCountryFromDatabase_WhenCacheMissAndDatabaseHit()
    {
        // Arrange
        const string cacheKey = "All_Countries";
        _mockCacheService.Setup(s => s.Get<IEnumerable<CountryDto>>(cacheKey)).Returns((IEnumerable<CountryDto>) null);

        var externalCountryInfo = new List<ExternalCountryInfo>
        {
            new()
            {
                Name = new CountryNames {Common = "TestCountry"},
                Borders = new List<string> {"Border1"},
                Capital = new List<string> {"TestCapital"}
            }
        };
        var jsonString = JsonSerializer.Serialize(externalCountryInfo);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        SetupHttpClientMock(mockHttpMessageHandler, HttpStatusCode.OK, jsonString);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient("restCountriesApiClient")).Returns(httpClient);

        var country = new Country {CommonName = "TestCountry", Capital = "TestCapital"};
        _mockCountryRepository.Setup(r => r.GetCountryByNameAsync("TestCountry", It.IsAny<CancellationToken>()))
            .ReturnsAsync(country);

        var countryDto = new CountryDto
            {CommonName = "TestCountry", Capital = "TestCapital", Borders = new List<string> {"Border1"}};
        _mockMapper.Setup(m => m.Map<CountryDto>(It.IsAny<Country>())).Returns(countryDto);

        // Mock BorderService's CreateOrUpdateBorderAsync method
        var borderDto = new BorderDto {BorderCode = "Border1"};
        _mockBorderService.Setup(s => s.CreateOrUpdateBorderAsync(It.IsAny<Border>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(borderDto);
        // Act
        var result = await _countryService.RetrieveAndSaveCountriesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal("TestCountry", resultList[0].CommonName);
        _mockCacheService.Verify(s => s.Set(cacheKey, It.IsAny<IEnumerable<CountryDto>>(), It.IsAny<TimeSpan>()),
            Times.Once);
    }

    /// <summary>
    /// Tests whether RetrieveAndSaveCountriesAsync correctly creates and returns new country data
    /// when both the cache and database do not have the requested country information.
    /// </summary>
    /// <remarks>
    /// This test simulates a scenario where no existing country data is available in the cache or the database.
    /// It checks if the method successfully retrieves data from an external API, creates new country records,
    /// and returns the correct country information. This test validates the functionality of the method
    /// in handling cases where it needs to fetch and store new data from external sources.
    /// </remarks>
    [Fact]
    public async Task RetrieveAndSaveCountriesAsync_ShouldCreateAndReturnNewCountries_WhenCacheAndDatabaseMiss()
    {
        // Arrange
        const string cacheKey = "All_Countries";
        _mockCacheService.Setup(s => s.Get<IEnumerable<CountryDto>>(cacheKey)).Returns((IEnumerable<CountryDto>) null);

        var externalCountryInfos = new List<ExternalCountryInfo>
        {
            new()
            {
                Name = new CountryNames {Common = "NewCountry"},
                Borders = new List<string> {"Border1"},
                Capital = new List<string> {"NewCapital"}
            }
        };
        var jsonString = JsonSerializer.Serialize(externalCountryInfos);


        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        SetupHttpClientMock(mockHttpMessageHandler, HttpStatusCode.OK, jsonString);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient("restCountriesApiClient")).Returns(httpClient);

        _mockCountryRepository.Setup(r => r.GetCountryByNameAsync("NewCountry", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Country) null);

        var newCountry = new Country {CommonName = "NewCountry", Capital = "NewCapital"};
        _mockCountryRepository.Setup(r => r.CreateCountryAsync(It.IsAny<Country>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newCountry);

        var countryDto = new CountryDto
            {CommonName = "NewCountry", Capital = "NewCapital", Borders = new List<string> {"Border1"}};
        _mockMapper.Setup(m => m.Map<CountryDto>(It.IsAny<Country>())).Returns(countryDto);

        var borderDto = new BorderDto {BorderCode = "Border1"};
        _mockBorderService.Setup(s => s.CreateOrUpdateBorderAsync(It.IsAny<Border>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(borderDto);

        // Act
        var result = await _countryService.RetrieveAndSaveCountriesAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Equal("NewCountry", resultList[0].CommonName);
        _mockCacheService.Verify(s => s.Set(cacheKey, It.IsAny<IEnumerable<CountryDto>>(), It.IsAny<TimeSpan>()),
            Times.Once);
    }

    /// <summary>
    /// Tests whether RetrieveAndSaveCountriesAsync correctly handles a failure in the external API call.
    /// </summary>
    /// <remarks>
    /// Simulates a scenario where the call to the external RestCountries API results in an internal server error.
    /// Verifies that the method properly handles this exception, ensuring robustness in case of external API disruptions.
    /// </remarks>
    [Fact]
    public async Task RetrieveAndSaveCountriesAsync_ShouldThrowHttpRequestException_WhenExternalApiFails()
    {
        // Arrange
        const string cacheKey = "All_Countries";
        _mockCacheService.Setup(s => s.Get<IEnumerable<CountryDto>>(cacheKey)).Returns((IEnumerable<CountryDto>) null);

        // Simulate an API failure
        const string internalServerError = "Internal Server Error";

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        SetupHttpClientMock(mockHttpMessageHandler, HttpStatusCode.InternalServerError, internalServerError);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient("restCountriesApiClient")).Returns(httpClient);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() =>
            _countryService.RetrieveAndSaveCountriesAsync(CancellationToken.None));

        Assert.IsType<HttpRequestException>(exception);
    }

    private class InvalidJsonData : TheoryData<string>
    {
        public InvalidJsonData()
        {
            Add("Invalid JSON Content"); // Invalid JSON
            Add(""); // Empty string
        }
    }

    /// <summary>
    /// Tests whether RetrieveAndSaveCountriesAsync correctly handles deserialization failures with invalid JSON data.
    /// </summary>
    /// <remarks>
    /// This test uses theory data to simulate scenarios with invalid JSON content, including an empty string and
    /// malformed JSON. It verifies that the method throws a JsonException, ensuring robust error handling
    /// for incorrect response data formats.
    /// </remarks>
    [Theory]
    [ClassData(typeof(InvalidJsonData))]
    public async Task RetrieveAndSaveCountriesAsync_ShouldThrowJsonException_WhenDeserializationFails(
        string invalidJsonString)
    {
        // Arrange
        const string cacheKey = "All_Countries";
        _mockCacheService.Setup(s => s.Get<IEnumerable<CountryDto>>(cacheKey)).Returns((IEnumerable<CountryDto>) null);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        SetupHttpClientMock(mockHttpMessageHandler, HttpStatusCode.OK, invalidJsonString);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(f => f.CreateClient("restCountriesApiClient")).Returns(httpClient);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() =>
            _countryService.RetrieveAndSaveCountriesAsync(CancellationToken.None));

        Assert.IsType<JsonException>(exception);
    }

    /// <summary>
    /// Sets up the mock HTTP message handler to return a specific HTTP response.
    /// </summary>
    /// <param name="mockHttpMessageHandler">The mock HTTP message handler.</param>
    /// <param name="statusCode">The HTTP status code to be returned in the response.</param>
    /// <param name="content">The content to be included in the response body.</param>
    /// <remarks>
    /// This method centralizes the setup for HTTP response mocking, ensuring consistency and reducing redundancy 
    /// across different tests. By following the DRY (Don't Repeat Yourself) principle, it helps in maintaining 
    /// cleaner and more maintainable test code.
    /// </remarks>
    private static void SetupHttpClientMock(Mock<HttpMessageHandler> mockHttpMessageHandler, HttpStatusCode statusCode,
        string content)
    {
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            });
    }

    // Note: The RetrieveAndSaveCountryByNameAsync method also requires comprehensive testing similar to
    // RetrieveAndSaveCountriesAsync, encompassing all the scenarios previously discussed. Although not
    // elaborated here due to their similar nature and because RetrieveAndSaveCountryByNameAsync wasn't
    // a primary focus of the initial challenge, it's important to ensure this method is thoroughly tested
    // for robustness and reliability. ^_^
}