using Common.Logging.Serilog.Factories.Abstractions;
using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Logging;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Services;

public class CountryBorderService : ICountryBorderService
{
    private readonly ICountryBorderRepository _countryBorderRepository;
    private readonly IMemoryCacheService _cacheService;
    private readonly ILogger<CountryBorderService> _logger;
    private readonly IEventIdFactory _eventIdFactory;

    public CountryBorderService(ICountryBorderRepository countryBorderRepository,
        IMemoryCacheService cacheService,
        ILogger<CountryBorderService> logger,
        IEventIdFactory eventIdFactory)
    {
        _countryBorderRepository = countryBorderRepository;
        _cacheService = cacheService;
        _logger = logger;
        _eventIdFactory = eventIdFactory;
    }

    public async Task AssociateCountryAndBorderAsync(int countryId, int borderId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"countryBorder_{countryId}_{borderId}";
        var cachedCountryBorder = _cacheService.Get<CountryBorder>(cacheKey);

        if (cachedCountryBorder == null)
        {
            var existingCountryBorder =
                await _countryBorderRepository.GetAsync(countryId, borderId, cancellationToken);
            if (existingCountryBorder == null)
            {
                var newCountryBorder = new CountryBorder
                {
                    CountryId = countryId,
                    BorderId = borderId
                };
                existingCountryBorder = await _countryBorderRepository.CreateAsync(newCountryBorder, cancellationToken);
                await _countryBorderRepository.CommitAsync(cancellationToken);
            }

            _cacheService.Set(cacheKey, existingCountryBorder, TimeSpan.FromMinutes(5));
        }
    }
}