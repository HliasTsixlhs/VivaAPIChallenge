using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;
using Microsoft.Extensions.Logging;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.Core.Exceptions;

namespace Viva.Geo.API.Core.Services;

/// <summary>
/// Service for processing arrays.
/// </summary>
public class ArrayProcessingService : IArrayProcessingService
{
    private readonly ILogger<ArrayProcessingService> _logger;
    private readonly IEventIdFactory _eventIdFactory;

    public ArrayProcessingService(ILogger<ArrayProcessingService> logger, IEventIdFactory eventIdFactory)
    {
        _logger = logger;
        _eventIdFactory = eventIdFactory;
    }

    /// <summary>
    /// Finds the second largest unique integer in a given array of integers.
    /// </summary>
    /// <remarks>
    /// This method is designed to identify the second largest distinct integer value.
    /// If the array contains less than two unique integers, the method returns -1.
    /// For example, an array like [5, 5, 5, 5, 5] will result in -1 as there is no second distinct integer.
    /// </remarks>
    /// <param name="numbers">The array of integers to process.</param>
    /// <returns>The second largest unique integer in the array, or -1 if there are less than two unique integers.</returns>
    public int FindSecondLargest(IEnumerable<int> numbers)
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.ArrayProcessing);
        _logger.LogInformation(
            eventId: eventId,
            message: "Finding the second largest number in the array.");

        var enumerable = numbers as int[] ?? numbers.ToArray();
        if (enumerable.Distinct().Count() < 2)
        {
            const string errorMessage = "Insufficient number of distinct elements in the array for processing.";

            var exception = new InsufficientUniqueElementsException(errorMessage);
            _logger.LogError(
                eventId: eventId,
                exception: exception,
                message: errorMessage);

            throw new InsufficientUniqueElementsException(errorMessage);
        }

        var secondLargest = enumerable.OrderByDescending(n => n).Distinct().Skip(1).First();
        _logger.LogInformation(
            eventId: eventId,
            message: "The second largest number found: {SecondLargest}", secondLargest);

        return secondLargest;
    }
}