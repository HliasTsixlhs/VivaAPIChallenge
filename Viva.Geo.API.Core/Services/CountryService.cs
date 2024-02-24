using System.Text.Json;
using AutoMapper;
using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;
using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Logging;
using Viva.Geo.API.Common.Dtos.Borders.Responses;
using Viva.Geo.API.Common.Dtos.Countries.Responses;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.Core.Models;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;
    private readonly ICountryBorderService _countryBorderService;
    private readonly IBorderService _borderService;
    private readonly IMemoryCacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CountryService> _logger;
    private readonly IEventIdFactory _eventIdFactory;

    public CountryService(
        ICountryRepository countryRepository,
        ICountryBorderService countryBorderService,
        IBorderService borderService,
        IMemoryCacheService cacheService,
        IMapper mapper,
        IHttpClientFactory httpClientFactory,
        ILogger<CountryService> logger,
        IEventIdFactory eventIdFactory)
    {
        _countryRepository = countryRepository;
        _countryBorderService = countryBorderService;
        _borderService = borderService;
        _cacheService = cacheService;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _eventIdFactory = eventIdFactory;
    }

    public async Task<CountryDto> RetrieveAndSaveCountryByNameAsync(string name,
        CancellationToken cancellationToken = default)
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.CountryProcessing);
        var cacheKey = $"Country_{name}";
        var cachedCountry = _cacheService.Get<CountryDto>(cacheKey);


        if (cachedCountry != null)
        {
            _logger.LogInformation(eventId: eventId,
                message: "Retrieved country data for {name} from cache", name);
            return cachedCountry;
        }

        try
        {
            var client = _httpClientFactory.CreateClient("restCountriesApiClient");
            var response =
                await client.GetAsync($"https://restcountries.com/v3.1/name/{name}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var externalCountryInfos = JsonSerializer.Deserialize<List<ExternalCountryInfo>>(content,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            if (externalCountryInfos == null) return null;
            foreach (var info in externalCountryInfos)
            {
                var existingCountry =
                    await _countryRepository.GetCountryByNameAsync(info.Name.Common, cancellationToken);

                Country savedCountry;
                if (existingCountry == null)
                {
                    var country = _mapper.Map<Country>(info);
                    savedCountry = await _countryRepository.CreateCountryAsync(country, cancellationToken);
                    await _countryRepository.CommitAsync(cancellationToken);
                }
                else
                {
                    savedCountry = existingCountry;
                }

                if (info.Borders == null) continue;
                var borders = new List<BorderDto>();
                foreach (var borderCode in info.Borders)
                {
                    var savedBorder =
                        await _borderService.CreateOrUpdateBorderAsync(new Border {BorderCode = borderCode},
                            cancellationToken);

                    await _countryBorderService.AssociateCountryAndBorderAsync(savedCountry.CountryId,
                        savedBorder.BorderId, cancellationToken);
                    borders.Add(savedBorder);
                }

                var result = _mapper.Map<CountryDto>(savedCountry);
                result.Borders = borders.Select(b => b.BorderCode).ToList();

                _logger.LogInformation(
                    eventId: eventId,
                    message: "Setting country data for {name} in cache", name);
                _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(5));
                return result;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                eventId: eventId,
                exception: ex,
                message: "HttpRequestException occurred while calling external API.");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(
                eventId: eventId,
                exception: ex,
                message: "JsonException occurred while trying to deserialize external's API data.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(eventId: eventId,
                exception: ex,
                message: "An error occurred during API call or deserialization.");
            throw;
        }

        return null;
    }

    public async Task<IEnumerable<CountryDto>> RetrieveAndSaveCountriesAsync(
        CancellationToken cancellationToken = default)
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.CountryProcessing);

        const string cacheKey = "All_Countries";
        var cachedCountries = _cacheService.Get<IEnumerable<CountryDto>>(cacheKey);

        if (cachedCountries != null)
        {
            _logger.LogInformation(
                eventId: eventId,
                message: "Retrieved all countries from cache");
            return cachedCountries;
        }

        try
        {
            var client = _httpClientFactory.CreateClient("restCountriesApiClient");

            var response = await client.GetAsync("https://restcountries.com/v3.1/all", cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var externalCountryInfos = JsonSerializer.Deserialize<List<ExternalCountryInfo>>(content,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            if (externalCountryInfos == null) return null;
            var results = new List<CountryDto>();
            foreach (var info in externalCountryInfos)
            {
                var existingCountry =
                    await _countryRepository.GetCountryByNameAsync(info.Name.Common, cancellationToken);

                Country savedCountry;
                if (existingCountry == null)
                {
                    var country = _mapper.Map<Country>(info);
                    savedCountry = await _countryRepository.CreateCountryAsync(country, cancellationToken);
                    await _countryRepository.CommitAsync(cancellationToken);
                }
                else
                {
                    savedCountry = existingCountry;
                }

                if (info.Borders == null) continue;
                var borders = new List<BorderDto>();
                foreach (var borderCode in info.Borders)
                {
                    var savedBorder =
                        await _borderService.CreateOrUpdateBorderAsync(new Border {BorderCode = borderCode},
                            cancellationToken);

                    await _countryBorderService.AssociateCountryAndBorderAsync(savedCountry.CountryId,
                        savedBorder.BorderId, cancellationToken);
                    borders.Add(savedBorder);
                }

                var result = _mapper.Map<CountryDto>(savedCountry);
                result.Borders = borders.Select(b => b.BorderCode).ToList();
                results.Add(result);
            }

            _logger.LogInformation(
                eventId: eventId,
                message: "Setting all countries data in cache");
            _cacheService.Set(cacheKey, results, TimeSpan.FromMinutes(5));
            return results;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                eventId: eventId,
                exception: ex,
                message: "HttpRequestException occurred while calling external API.");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(
                eventId: eventId,
                exception: ex,
                message: "JsonException occurred while trying to deserialize external's API data.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(eventId: eventId,
                exception: ex,
                message: "An error occurred during API call or deserialization.");
            throw;
        }
    }
}