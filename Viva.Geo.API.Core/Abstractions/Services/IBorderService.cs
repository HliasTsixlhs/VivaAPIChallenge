using Viva.Geo.API.Common.Dtos.Borders.Responses;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Abstractions.Services;

public interface IBorderService
{
    Task<BorderDto> CreateOrUpdateBorderAsync(Border border, CancellationToken cancellationToken = default);
}