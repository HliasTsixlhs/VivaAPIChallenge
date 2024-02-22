using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;
using Microsoft.Extensions.Logging;
using Viva.Geo.API.Core.Abstractions.Services;

namespace Viva.Geo.API.Core.Services;

/// <summary>
/// Service for processing arrays and finding the second largest number.
/// </summary>
public class SecondLargestNumberService : ISecondLargestNumberService
{
    private readonly ILogger<SecondLargestNumberService> _logger;
    private readonly IEventIdFactory _eventIdFactory;

    public SecondLargestNumberService(ILogger<SecondLargestNumberService> logger, IEventIdFactory eventIdFactory)
    {
        _logger = logger;
        _eventIdFactory = eventIdFactory;
    }

    /// <summary>
    /// Finds the second largest number in an array using LINQ.
    /// </summary>
    /// <param name="numbers">The array of integers to process.</param>
    /// <returns>The second largest integer in the array.</returns>
    public int FindSecondLargest(IEnumerable<int> numbers)
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.ArrayProcessing);
        _logger.LogInformation(eventId, "Finding the second largest number in the array.");

        var secondLargest = numbers
            .OrderByDescending(n => n)
            .Distinct()
            .Skip(1)
            .FirstOrDefault();

        _logger.LogInformation(eventId, "The second largest number found: {SecondLargest}", secondLargest);

        return secondLargest;
    }
}