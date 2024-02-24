using Viva.Geo.API.Common.Dtos.Countries.Responses;

namespace Viva.Geo.API.Core.Abstractions.Services;

public interface ICountryService
{
    /// <summary>
    /// Asynchronously retrieves and saves country data by name. 
    /// First checks the cache for existing data. If not found, fetches data from an external API.
    /// </summary>
    /// <param name="name">The name of the country to retrieve.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <remarks>
    /// This method attempts to retrieve country data from cache based on the provided name.
    /// If the data is not available in cache, it fetches the data from an external API (restcountries.com).
    /// After retrieval, the data is mapped to a CountryDto, saved in cache, and returned.
    /// Handles HttpRequestException and JsonException for issues during API call or deserialization.
    /// </remarks>
    /// <returns>
    /// The CountryDto object containing the country data, or null if no data is found or an error occurs.
    /// </returns>
    Task<CountryDto> RetrieveAndSaveCountryByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves and saves a list of countries asynchronously. It first checks if the list of countries is available 
    /// in the cache. If not, it fetches the countries from an external API, maps them to CountryDto objects, 
    /// saves the new or updates existing countries in the database, and caches the results.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, resulting in a list of CountryDto objects.</returns>
    /// <remarks>
    /// This method handles all aspects of retrieving country information, including external API calls,
    /// deserialization, caching, and database operations. It uses exception handling to manage any errors 
    /// during the API call or deserialization process.
    /// </remarks>
    Task<IEnumerable<CountryDto>> RetrieveAndSaveCountriesAsync(CancellationToken cancellationToken = default);
}