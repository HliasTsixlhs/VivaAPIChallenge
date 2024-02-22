using Common.Persistence.EFCore;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.DataAccess.Context;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Repositories;

public class BorderRepository : BaseRepository<Border, int, GeoContext>, IBorderRepository
{
    public BorderRepository(GeoContext context) : base(context)
    {
    }
}