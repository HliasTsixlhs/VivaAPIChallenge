using AutoMapper;
using Common.Logging.Serilog.Factories.Abstractions;
using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Viva.Geo.API.Common.Dtos.Countries.Responses;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.Core.Services;

namespace Viva.Geo.API.Core.Tests.Services;

public class CountryServiceTests
{
    private readonly Mock<ICountryRepository> _mockCountryRepository;
    private readonly Mock<ICountryBorderService> _mockCountryBorderService;
    private readonly Mock<IBorderService> _mockBorderService;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<ILogger<CountryService>> _mockLogger;
    private readonly Mock<IEventIdFactory> _mockEventIdFactory;
    private readonly CountryService _countryService;

    public CountryServiceTests()
    {
        _mockCountryRepository = new Mock<ICountryRepository>();
        _mockCountryBorderService = new Mock<ICountryBorderService>();
        _mockBorderService = new Mock<IBorderService>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _mockMapper = new Mock<IMapper>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<CountryService>>();
        _mockEventIdFactory = new Mock<IEventIdFactory>();

        _countryService = new CountryService(
            _mockCountryRepository.Object,
            _mockCountryBorderService.Object,
            _mockBorderService.Object,
            _mockCacheService.Object,
            _mockMapper.Object,
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockEventIdFactory.Object);
    }

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

    // [Fact]
    // public async Task
    //     RetrieveAndSaveCountriesAsync_ShouldReturnExistingCountryFromDatabase_WhenCacheMissAndDatabaseHit()
    // {
    //     // Arrange
    //     const string cacheKey = "All_Countries";
    //     _mockCacheService.Setup(s => s.Get<IEnumerable<CountryDto>>(cacheKey)).Returns((IEnumerable<CountryDto>) null);
    //
    //     const string countryName = "TestCountry";
    //     var countryInfo = new ExternalCountryInfo
    //     {
    //         Name = new ExternalCountryInfo.CountryNames {Common = countryName}, Borders = new List<string> {"Border1"}
    //     };
    //     var countryDto = new CountryDto {CommonName = countryName};
    //
    //     var externalCountryInfo = new ExternalCountryInfo
    //     {
    //         Name = new ExternalCountryInfo.CountryNames {Common = "TestCountry"},
    //         Borders = new List<string> {"Border1"},
    //         Capital = new List<string> {"TestCapital"}
    //     };
    //     var country = new Country {CommonName = countryName, Capital = "TestCapital"};
    //
    //     var countryInfoList = new List<ExternalCountryInfo> {externalCountryInfo};
    //     var jsonString = JsonSerializer.Serialize(countryInfoList);
    //
    //     var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    //     var response = new HttpResponseMessage
    //     {
    //         StatusCode = HttpStatusCode.OK,
    //         Content = new StringContent(jsonString),
    //     };
    //
    //     mockHttpMessageHandler
    //         .Protected()
    //         .Setup<Task<HttpResponseMessage>>(
    //             "SendAsync",
    //             ItExpr.IsAny<HttpRequestMessage>(),
    //             ItExpr.IsAny<CancellationToken>()
    //         )
    //         .ReturnsAsync(response);
    //
    //     var httpClient = new HttpClient(mockHttpMessageHandler.Object);
    //     _mockHttpClientFactory.Setup(_ => _.CreateClient("restCountriesApiClient")).Returns(httpClient);
    //
    //     _mockCountryRepository.Setup(r => r.GetCountryByNameAsync(countryName, It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(country);
    //     _mockMapper.Setup(m => m.Map<CountryDto>(It.IsAny<Country>())).Returns(countryDto);
    //
    //     // Act
    //     var result = await _countryService.RetrieveAndSaveCountriesAsync(CancellationToken.None);
    //
    //     // Assert
    //     Assert.NotNull(result);
    //     var resultList = result.ToList();
    //     Assert.Single(resultList);
    //     Assert.Equal(countryName, resultList[0].CommonName);
    //     _mockCacheService.Verify(s => s.Set(cacheKey, It.IsAny<IEnumerable<CountryDto>>(), It.IsAny<TimeSpan>()),
    //         Times.Once);
    // }


    // [Fact]
    // public async Task RetrieveAndSaveCountriesAsync_ShouldCreateAndReturnNewCountries_WhenCacheAndDatabaseMiss()
    // {
    //     // Arrange
    //     var mockCountries = new List<ExternalCountryInfo> { /* Populate with test data */ };
    //     var expectedCountries = new List<CountryDto> { /* Populate with expected DTOs */ };
    //     var httpClientMock = new Mock<HttpClient>();
    //     var httpResponse = new HttpResponseMessage
    //     {
    //         StatusCode = HttpStatusCode.OK,
    //         Content = new StringContent(JsonSerializer.Serialize(mockCountries))
    //     };
    //     httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(httpResponse);
    //     _mockCacheService.Setup(s => s.Get<IEnumerable<CountryDto>>(It.IsAny<string>()))
    //         .Returns((IEnumerable<CountryDto>)null);
    //     _mockCountryRepository.Setup(r => r.GetCountryByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync((Country)null);
    //     _mockMapper.Setup(m => m.Map<Country>(It.IsAny<ExternalCountryInfo>()))
    //         .Returns(/* Logic to transform ExternalCountryInfo to Country */);
    //     _mockMapper.Setup(m => m.Map<CountryDto>(It.IsAny<Country>()))
    //         .Returns(/* Logic to transform Country to CountryDto */);
    //
    //     // Act
    //     var result = await _countryService.RetrieveAndSaveCountriesAsync(CancellationToken.None);
    //
    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.Equal(expectedCountries.Count, result.Count());
    //     _mockCountryRepository.Verify(r => r.CreateCountryAsync(It.IsAny<Country>(), It.IsAny<CancellationToken>()), Times.Exactly(mockCountries.Count));
    //     _mockCacheService.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<IEnumerable<CountryDto>>(), It.IsAny<TimeSpan>()), Times.Once);
    // }
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
                Name = new ExternalCountryInfo.CountryNames {Common = "TestCountry"},
                Borders = new List<string> {"Border1"},
                Capital = new List<string> {"TestCapital"}
            }
        };
        var jsonString = JsonSerializer.Serialize(externalCountryInfo);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonString)
        };

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

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
            new ExternalCountryInfo
            {
                Name = new ExternalCountryInfo.CountryNames {Common = "NewCountry"},
                Borders = new List<string> {"Border1"},
                Capital = new List<string> {"NewCapital"}
            }
        };
        var jsonString = JsonSerializer.Serialize(externalCountryInfos);


        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonString)
        };

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

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
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            });

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
}