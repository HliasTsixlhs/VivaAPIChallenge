using Microsoft.AspNetCore.Mvc;
using Viva.Geo.API.Core.Abstractions.Services;

namespace Viva.Geo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountriesController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    public async Task<IActionResult> RetrieveAndSaveCountries()
    {
        var countries = await _countryService.RetrieveAndSaveCountriesAsync();
        if (countries == null || !countries.Any())
        {
            return NotFound("No countries data found.");
        }

        return Ok(countries);
    }

    [HttpGet("{countryName}")]
    public async Task<IActionResult> RetrieveAndSaveCountry(string countryName)
    {
        var countryDto = await _countryService.RetrieveAndSaveCountryByNameAsync(countryName);
        if (countryDto == null)
        {
            return NotFound($"Country data for {countryName} not found.");
        }

        return Ok(countryDto);
    }
}