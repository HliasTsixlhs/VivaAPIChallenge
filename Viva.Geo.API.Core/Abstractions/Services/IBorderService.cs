using Viva.Geo.API.Common.Dtos.Borders.Responses;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Abstractions.Services;

public interface IBorderService
{
    /// <summary>
    /// Asynchronously creates or updates a border entry and caches it.
    /// </summary>
    /// <param name="border">The border entity to be created or updated.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <remarks>
    /// This method checks if the border entity exists in the cache. If it's found, it's returned directly.
    /// Otherwise, the repository is searched for an existing border with the same code. If not found,
    /// a new border is created. The method ensures that the border data is up-to-date in the cache.
    /// </remarks>
    /// <returns>
    /// A task resulting in the DTO of the created or updated border.
    /// </returns>
    Task<BorderDto> CreateOrUpdateBorderAsync(Border border, CancellationToken cancellationToken = default);
}