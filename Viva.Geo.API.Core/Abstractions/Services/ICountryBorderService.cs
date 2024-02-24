namespace Viva.Geo.API.Core.Abstractions.Services;

public interface ICountryBorderService
{
    /// <summary>
    /// Asynchronously associates a country with a border and caches the association.
    /// </summary>
    /// <param name="countryId">The ID of the country.</param>
    /// <param name="borderId">The ID of the border.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <remarks>
    /// This method first checks the cache for an existing country-border association.
    /// If not found in cache, it queries the repository. If the association is still not found,
    /// a new one is created and saved in the repository. Finally, the association is cached.
    /// </remarks>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    Task AssociateCountryAndBorderAsync(int countryId, int borderId, CancellationToken cancellationToken = default);
}