using Viva.Geo.API.DataAccess.DataAccessModels;
using Common.Persistence.EFCore.Abstractions;

namespace Viva.Geo.API.Core.Abstractions.Repositories;

public interface ICountryBorderRepository : IEfCoreRepository<CountryBorder, (int CountryId, int BorderId)>
{
    Task<CountryBorder> GetAsync(int countryId, int borderId, CancellationToken cancellationToken = default);
}