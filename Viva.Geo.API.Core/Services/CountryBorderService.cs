using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using AutoMapper;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Services;

public class CountryBorderService : ICountryBorderService
{
    private readonly ICountryBorderRepository _countryBorderRepository;
    private readonly IMapper _mapper;

    public CountryBorderService(ICountryBorderRepository countryBorderRepository, IMapper mapper)
    {
        _countryBorderRepository = countryBorderRepository;
        _mapper = mapper;
    }

    public async Task AssociateCountryAndBorderAsync(int countryId, int borderId,
        CancellationToken cancellationToken = default)
    {
        // Check if the association already exists
        var existingAssociation = await _countryBorderRepository.GetAsync(countryId, borderId, cancellationToken);
        if (existingAssociation != null)
        {
            // Association already exists, no action needed
            return;
        }

        // Create new association
        var newAssociation = new CountryBorder
        {
            CountryId = countryId,
            BorderId = borderId
        };

        // Save the new association
        await _countryBorderRepository.CreateAsync(newAssociation, cancellationToken);
        await _countryBorderRepository.CommitAsync(cancellationToken);
    }
}