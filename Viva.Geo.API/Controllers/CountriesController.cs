using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Viva.Geo.API.Core.Abstractions.Services;

namespace Viva.Geo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;
    private readonly ILogger<CountriesController> _logger;
    private readonly IEventIdFactory _eventIdFactory;

    public CountriesController(ICountryService countryService,
        ILogger<CountriesController> logger,
        IEventIdFactory eventIdFactory)
    {
        _countryService = countryService;
        _logger = logger;
        _eventIdFactory = eventIdFactory;
    }

    [HttpGet]
    public async Task<IActionResult> RetrieveAndSaveCountries()
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.CountryProcessing);
        var traceId = HttpContext.TraceIdentifier;

        using (_logger.BeginScope("TraceId: {TraceId}", traceId))
        {
            _logger.LogInformation(eventId, "Retrieving and saving countries. TraceId: {TraceId}", traceId);
            var countries = await _countryService.RetrieveAndSaveCountriesAsync();
            if (countries == null || !countries.Any())
            {
                return NotFound("No countries data found.");
            }

            _logger.LogInformation(eventId, "Successfully retrieved and saved countries data. TraceId: {TraceId}",
                traceId);
            return Ok(countries);
        }
    }

    [HttpGet("{countryName}")]
    public async Task<IActionResult> RetrieveAndSaveCountry(string countryName)
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.CountryProcessing);
        var traceId = HttpContext.TraceIdentifier;

        using (_logger.BeginScope("TraceId: {TraceId}", traceId))
        {
            _logger.LogInformation(eventId, "Retrieving and saving country for: {countryName}. TraceId: {TraceId}",
                countryName, traceId);

            var countryDto = await _countryService.RetrieveAndSaveCountryByNameAsync(countryName);
            if (countryDto == null)
            {
                return NotFound($"Country data for {countryName} not found.");
            }

            _logger.LogInformation(eventId,
                "Successfully retrieved and saved country data for: {countryName}. TraceId: {TraceId}", countryName,
                traceId);
            return Ok(countryDto);
        }
    }
}