using Common.Persistence.EFCore.Abstractions;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Abstractions.Repositories;

public interface ICountryRepository : IEfCoreRepository<Country, int>
{
    Task<Country> GetCountryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Country> CreateCountryAsync(Country country, CancellationToken cancellationToken = default);
}