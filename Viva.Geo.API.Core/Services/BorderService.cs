using AutoMapper;
using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;
using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<BorderService> _logger;
    private readonly IEventIdFactory _eventIdFactory;

    public BorderService(IBorderRepository borderRepository, IMemoryCacheService cacheService, IMapper mapper,
        ILogger<BorderService> logger, IEventIdFactory eventIdFactory)
    {
        _borderRepository = borderRepository;
        _cacheService = cacheService;
        _mapper = mapper;
        _logger = logger;
        _eventIdFactory = eventIdFactory;
    }

    public async Task<BorderDto> CreateOrUpdateBorderAsync(Border border, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"border_{border.BorderCode}";
        var cachedBorderDto = _cacheService.Get<BorderDto>(cacheKey);

        if (cachedBorderDto != null)
        {
            _logger.LogInformation(_eventIdFactory.Create(VivaGeoApiEvent.BorderProcessing),
                "Fetched border with code {BorderCode} from cache", border.BorderCode);
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
            _logger.LogInformation(_eventIdFactory.Create(VivaGeoApiEvent.BorderProcessing),
                "Created new border with code {BorderCode}", border.BorderCode);
        }
        else
        {
            existingBorder = borders.First();
            _logger.LogInformation(_eventIdFactory.Create(VivaGeoApiEvent.BorderProcessing),
                "Existing border with code {BorderCode} found, no new creation", border.BorderCode);
        }

        var borderDto = _mapper.Map<BorderDto>(existingBorder);
        _cacheService.Set(cacheKey, borderDto, TimeSpan.FromMinutes(5));
        _logger.LogInformation(_eventIdFactory.Create(VivaGeoApiEvent.BorderProcessing),
            "Updated cache for border with code {BorderCode}", border.BorderCode);

        return borderDto;
    }
}