using Viva.Geo.API.Core.Abstractions.Services;

namespace Viva.Geo.API.Core.Services;

/// <summary>
/// Service for processing arrays and finding the second largest number.
/// </summary>
public class SecondLargestNumberService : ISecondLargestNumberService
{
    /// <summary>
    /// Finds the second largest number in an array using LINQ.
    /// </summary>
    /// <param name="numbers">The array of integers to process.</param>
    /// <returns>The second largest integer in the array.</returns>
    public int FindSecondLargest(IEnumerable<int> numbers)
    {
        return numbers
            .OrderByDescending(n => n)
            .Distinct()
            .Skip(1)
            .FirstOrDefault();
    }
}