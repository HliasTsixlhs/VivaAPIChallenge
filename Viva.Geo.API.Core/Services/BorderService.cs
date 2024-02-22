using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.DataAccess.DataAccessModels;
using AutoMapper;
using Viva.Geo.API.Common.Dtos.Borders.Responses;

namespace Viva.Geo.API.Core.Services;

public class BorderService : IBorderService
{
    private readonly IBorderRepository _borderRepository;
    private readonly IMapper _mapper;

    public BorderService(IBorderRepository borderRepository, IMapper mapper)
    {
        _borderRepository = borderRepository;
        _mapper = mapper;
    }

    public async Task<BorderDto> CreateOrUpdateBorderAsync(Border border, CancellationToken cancellationToken = default)
    {
        // Check if the border already exists
        var existingBorder =
            await _borderRepository.SearchAsync(b => b.BorderCode == border.BorderCode, cancellationToken);

        var enumerable = existingBorder as Border[] ?? existingBorder.ToArray();
        if (enumerable.Any())
        {
            // Border already exists, so return the existing one
            return _mapper.Map<BorderDto>(enumerable.FirstOrDefault());
        }

        // Border does not exist, create a new one
        var createdBorder = await _borderRepository.CreateAsync(border, cancellationToken);
        await _borderRepository.CommitAsync(cancellationToken);
        return _mapper.Map<BorderDto>(createdBorder);
    }
}