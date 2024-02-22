﻿using System.Text.Json;
using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public CountryService(
        ICountryRepository countryRepository,
        ICountryBorderService countryBorderService,
        IBorderService borderService,
        IMapper mapper,
        HttpClient httpClient)
    {
        _countryRepository = countryRepository;
        _countryBorderService = countryBorderService;
        _borderService = borderService;
        _mapper = mapper;
        _httpClient = httpClient;
    }

    public async Task<CountryDto> RetrieveAndSaveCountryByNameAsync(string countryName,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _httpClient.GetAsync($"https://restcountries.com/v3.1/name/{countryName}", cancellationToken);
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
            return result;
        }

        return null;
    }

    public async Task<IEnumerable<CountryDto>> RetrieveAndSaveCountriesAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"https://restcountries.com/v3.1/all", cancellationToken);
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

        return results;
    }
}