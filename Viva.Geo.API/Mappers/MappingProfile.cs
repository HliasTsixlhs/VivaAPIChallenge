using AutoMapper;
using Viva.Geo.API.Common.Dtos.Borders.Responses;
using Viva.Geo.API.Common.Dtos.Countries.Responses;
using Viva.Geo.API.Core.Models;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ExternalCountryInfo, Country>()
            .ForMember(dest => dest.CommonName, opt => opt.MapFrom(src => src.Name.Common))
            .ForMember(dest => dest.Capital, opt => opt.MapFrom(src => src.Capital.FirstOrDefault()));

        // Mapping configuration for Country to CountryDto
        CreateMap<Country, CountryDto>()
            .ForMember(dest => dest.CommonName, opt => opt.MapFrom(src => src.CommonName))
            .ForMember(dest => dest.Capital, opt => opt.MapFrom(src => src.Capital.FirstOrDefault()));

        // Mapping configuration for Border to BorderDto
        CreateMap<Border, BorderDto>()
            .ForMember(dest => dest.BorderCode, opt => opt.MapFrom(src => src.BorderCode))
            .ForMember(dest => dest.BorderId, opt => opt.MapFrom(src => src.BorderId));
    }
}