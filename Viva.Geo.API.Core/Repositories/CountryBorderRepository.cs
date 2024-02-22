using Viva.Geo.API.DataAccess.Context;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.DataAccess.DataAccessModels;
using Common.Persistence.EFCore;

namespace Viva.Geo.API.Core.Repositories;

public class CountryBorderRepository : BaseRepository<CountryBorder, (int CountryId, int BorderId), GeoContext>,
    ICountryBorderRepository
{
    public CountryBorderRepository(GeoContext context) : base(context)
    {
    }

    public async Task<CountryBorder> GetAsync(int countryId, int borderId,
        CancellationToken cancellationToken = default)
    {
        var result = await SearchAsync(cb => cb.CountryId == countryId && cb.BorderId == borderId, cancellationToken);
        return result.FirstOrDefault();
    }
}