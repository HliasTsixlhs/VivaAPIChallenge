namespace Viva.Geo.API.Core.Abstractions.Services;

public interface ICountryBorderService
{
    Task AssociateCountryAndBorderAsync(int countryId, int borderId, CancellationToken cancellationToken = default);
}