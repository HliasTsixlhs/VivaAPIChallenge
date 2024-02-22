using Common.Persistence.EFCore;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.DataAccess.Context;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Repositories;

public class CountryRepository : BaseRepository<Country, int, GeoContext>, ICountryRepository
{
    public CountryRepository(GeoContext context) : base(context)
    {
    }

    public async Task<Country> GetCountryByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        // Use SearchAsync to find a country by its name
        var countries = await SearchAsync(c => c.CommonName == name, cancellationToken);
        return countries.FirstOrDefault();
    }

    public async Task<Country> CreateCountryAsync(Country country, CancellationToken cancellationToken = default)
    {
        var createdCountry = await CreateAsync(country, cancellationToken);
        return createdCountry;
    }
}