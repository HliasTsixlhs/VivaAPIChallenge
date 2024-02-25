using System.Net.Http.Json;
using Viva.Geo.API.Common.Dtos.ArrayProcessing.Requests;
using Viva.Geo.API.Common.Dtos.ArrayProcessing.Responses;
using Viva.Geo.API.IntegrationTests.Constants;
using Viva.Geo.API.IntegrationTests.WebApplicationFactory;

namespace Viva.Geo.API.IntegrationTests.Controllers;

[Collection("SequentialDatabaseTests")]
public class ArrayProcessingControllerTests : IDisposable
{
    private readonly VivaGeoWebApplicationFactory _app;
    private readonly HttpClient _client;

    public ArrayProcessingControllerTests()
    {
        _app = new VivaGeoWebApplicationFactory();
        _client = _app.CreateClient();
    }

    /// <summary>
    /// Tests the "Find Second Largest" endpoint to ensure it returns the correct second largest integer
    /// from an array of numbers. This test sends a request with multiple numbers and verifies
    /// that the response contains the expected second largest number.
    /// </summary>
    [Fact]
    public async Task FindSecondLargestEndpoint_ShouldReturnSecondLargestInteger_WhenGivenMultipleNumbers()
    {
        // Arrange
        var request = new SecondLargestIntegerRequest
        {
            Numbers = new[] {3, 2, 1}
        };

        // Act
        var response = await _client.PostAsJsonAsync(TestConstants.ArrayProcessingSecondLargestEndpoint, request);
        var responseData = await response.Content.ReadFromJsonAsync<SecondLargestIntegerResponse>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(responseData);
        Assert.Equal(2, responseData.SecondLargestInteger);
    }

    public void Dispose()
    {
        _app?.Dispose();
        _client?.Dispose();
    }
}