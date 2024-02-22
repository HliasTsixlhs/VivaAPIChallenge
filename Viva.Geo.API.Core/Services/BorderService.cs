using AutoMapper;
using Common.MemoryCaching.Abstractions;
using Viva.Geo.API.Common.Dtos.Borders.Responses;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Services;

public class BorderService : IBorderService
{
    private readonly IBorderRepository _borderRepository;
    private readonly IMemoryCacheService _cacheService;
    private readonly IMapper _mapper;

    public BorderService(IBorderRepository borderRepository, IMemoryCacheService cacheService, IMapper mapper)
    {
        _borderRepository = borderRepository;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<BorderDto> CreateOrUpdateBorderAsync(Border border, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"border_{border.BorderCode}";
        var cachedBorderDto = _cacheService.Get<BorderDto>(cacheKey);

        if (cachedBorderDto != null)
        {
            return cachedBorderDto;
        }

        var existingBorders =
            await _borderRepository.SearchAsync(b => b.BorderCode == border.BorderCode, cancellationToken);
        Border existingBorder;

        var borders = existingBorders.ToList();
        if (!borders.Any())
        {
            existingBorder = await _borderRepository.CreateAsync(border, cancellationToken);
            await _borderRepository.CommitAsync(cancellationToken);
        }
        else
        {
            existingBorder = borders.First();
        }

        var borderDto = _mapper.Map<BorderDto>(existingBorder);
        _cacheService.Set(cacheKey, borderDto, TimeSpan.FromMinutes(5));
        return borderDto;
    }
}