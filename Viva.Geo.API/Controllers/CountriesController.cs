using Asp.Versioning;
using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Viva.Geo.API.Core.Abstractions.Services;

namespace Viva.Geo.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
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
        var connectionRemotePort = HttpContext.Connection.RemotePort;

        using (_logger.BeginScope(new Dictionary<string, object> {["ConnectionRemotePort"] = connectionRemotePort}))
        {
            _logger.LogInformation(
                eventId: eventId,
                message: "Retrieving and saving countries.");

            var countries = await _countryService.RetrieveAndSaveCountriesAsync();
            if (countries == null || !countries.Any())
            {
                return NotFound("No countries data found.");
            }

            _logger.LogInformation(
                eventId: eventId,
                message: "Successfully retrieved and saved countries data.");

            return Ok(countries);
        }
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> RetrieveAndSaveCountry(string name)
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.CountryProcessing);
        var connectionRemotePort = HttpContext.Connection.RemotePort;


        using (_logger.BeginScope(new Dictionary<string, object> {["ConnectionRemotePort"] = connectionRemotePort}))
        {
            _logger.LogInformation(
                eventId: eventId,
                message: "Retrieving and saving country for: {name}.", name);

            var countryDto = await _countryService.RetrieveAndSaveCountryByNameAsync(name);
            if (countryDto == null)
            {
                return NotFound($"Country data for {name} not found.");
            }

            _logger.LogInformation(
                eventId: eventId,
                message: "Successfully retrieved and saved country data for: {name}.", name);

            return Ok(countryDto);
        }
    }
}