using Viva.Geo.API.Common.Dtos.Countries.Responses;

namespace Viva.Geo.API.Core.Abstractions.Services;

public interface ICountryService
{
    Task<CountryDto> RetrieveAndSaveCountryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<CountryDto>> RetrieveAndSaveCountriesAsync(CancellationToken cancellationToken = default);
}