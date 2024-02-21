namespace Viva.Geo.API.Core.Abstractions.Services;

/// <summary>
/// Interface for a service that provides functionality to find the second largest number in an array.
/// </summary>
public interface ISecondLargestNumberService
{
    /// <summary>
    /// Finds the second largest number in the provided array of integers.
    /// </summary>
    /// <param name="numbers">The array of integers to process.</param>
    /// <returns>The second largest integer in the array.</returns>
    int FindSecondLargest(IEnumerable<int> numbers);
}