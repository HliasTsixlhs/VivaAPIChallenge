﻿using System.Net.Http.Json;
using Viva.Geo.API.Common.Dtos.Countries.Responses;
using Viva.Geo.API.IntegrationTests.Constants;
using Viva.Geo.API.IntegrationTests.WebApplicationFactory;

namespace Viva.Geo.API.IntegrationTests.Controllers;

[Collection("SequentialDatabaseTests")]
public class CountriesControllerTests : IDisposable
{
    private readonly VivaGeoWebApplicationFactory _app;
    private readonly HttpClient _client;

    public CountriesControllerTests()
    {
        _app = new VivaGeoWebApplicationFactory();
        _client = _app.CreateClient();
    }

    /// <summary>
    /// Tests the "Retrieve and Save Country" endpoint to ensure that data is being cached effectively.
    /// The test makes two consecutive requests to the same endpoint and compares the response times.
    /// The expectation is that the second request should be served faster due to caching,
    /// thereby validating the caching mechanism of the endpoint.
    /// </summary>
    [Fact]
    public async Task RetrieveAndSaveCountryEndpoint_ShouldReturnCachedData_WhenCalledConsecutively()
    {
        // Arrange
        const string endpoint = $"{TestConstants.CountriesEndpoint}/greece";

        // Act
        var firstRequestStart = DateTime.Now;
        var firstResponse = await _client.GetFromJsonAsync<CountryDto>(endpoint);
        var firstRequestEnd = DateTime.Now;

        var secondRequestStart = DateTime.Now;
        var secondResponse = await _client.GetFromJsonAsync<CountryDto>(endpoint);
        var secondRequestEnd = DateTime.Now;

        // Calculate request times
        var firstRequestTime = firstRequestEnd - firstRequestStart;
        var secondRequestTime = secondRequestEnd - secondRequestStart;

        // Assert - Validate first response
        Assert.NotNull(firstResponse);
        Assert.Equal("Greece", firstResponse.CommonName);
        Assert.Equal("Athens", firstResponse.Capital);
        Assert.Contains("ALB", firstResponse.Borders);
        Assert.Contains("BGR", firstResponse.Borders);
        Assert.Contains("TUR", firstResponse.Borders);
        Assert.Contains("MKD", firstResponse.Borders);

        // Assert - Validate second response
        Assert.NotNull(secondResponse);

        // Assert - Compare times
        Assert.True(secondRequestTime < firstRequestTime, "Second request should be faster due to caching");
    }

    public void Dispose()
    {
        _app.Dispose();
        _client.Dispose();
    }
}