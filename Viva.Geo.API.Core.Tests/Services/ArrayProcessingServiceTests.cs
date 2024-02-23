using Moq;
using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;
using Microsoft.Extensions.Logging;
using Viva.Geo.API.Core.Exceptions;
using Viva.Geo.API.Core.Services;

namespace Viva.Geo.API.Core.Tests.Services;

public class ArrayProcessingServiceTests
{
    private readonly ArrayProcessingService _service;

    public ArrayProcessingServiceTests()
    {
        var mockLogger = new Mock<ILogger<ArrayProcessingService>>();
        var mockEventIdFactory = new Mock<IEventIdFactory>();

        // Mock behavior of EventIdFactory if required
        mockEventIdFactory.Setup(factory => factory.Create(It.IsAny<VivaGeoApiEvent>()))
            .Returns(new EventId(1, "TestEvent"));

        _service = new ArrayProcessingService(mockLogger.Object, mockEventIdFactory.Object);
    }


    [Theory]
    [InlineData(new[] {1, 2, 3, 4, 5}, 4)]
    [InlineData(new[] {5, 4, 3, 2, 1}, 4)]
    public void FindSecondLargest_ShouldReturnsCorrectValue_WhenInputArrayIsValid(int[] numbers, int expected)
    {
        // Act
        var result = _service.FindSecondLargest(numbers);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new[] {5, 5, 5, 5, 5})]
    [InlineData(new int[] { })]
    [InlineData(new[] {100})]
    public void FindSecondLargest_ShouldThrowsInsufficientUniqueElementsException_WhenInputArrayIsNotValid(
        int[] numbers)
    {
        // Act
        Action act = () => _service.FindSecondLargest(numbers);

        // Assert
        Assert.Throws<InsufficientUniqueElementsException>(act);
    }
}