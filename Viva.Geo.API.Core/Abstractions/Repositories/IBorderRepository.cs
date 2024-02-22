using Common.Persistence.EFCore.Abstractions;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Abstractions.Repositories;

public interface IBorderRepository : IEfCoreRepository<Border, int>
{
    // Task<Border> CreateBorderAsync(Border border, CancellationToken cancellationToken = default);
}