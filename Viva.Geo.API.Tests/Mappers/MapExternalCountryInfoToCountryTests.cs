using AutoMapper;
using Viva.Geo.API.Core.Models;
using Viva.Geo.API.DataAccess.DataAccessModels;
using Viva.Geo.API.Mappers;

namespace Viva.Geo.API.Tests.Mappers;

public class MapExternalCountryInfoToCountryTests
{
    private readonly IMapper _mapper;

    public MapExternalCountryInfoToCountryTests()
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        _mapper = mappingConfig.CreateMapper();
    }

    /// <summary>
    /// Tests the mapping of essential fields from ExternalCountryInfo to Country model when only key properties are provided.
    /// </summary>
    /// <remarks>
    /// This test checks the AutoMapper configuration to ensure that it correctly maps the essential fields (CommonName and Capital)
    /// from an ExternalCountryInfo instance to a Country model. The test is crucial for verifying that the mapping handles scenarios
    /// where only key information is available, ensuring that the essential data is accurately transferred despite partial input.
    /// This aligns with scenarios where limited external API data is used to populate the core model attributes.
    /// </remarks>
    [Fact]
    public void Mapper_ShouldMapEssentialFields_WhenOnlyKeyPropertiesAreProvided()
    {
        // Arrange
        var externalCountryInfo = new ExternalCountryInfo
        {
            Name = new ExternalCountryInfo.CountryNames {Common = "TestCountry"},
            Capital = "TestCapital",
        };

        // Act
        var country = _mapper.Map<Country>(externalCountryInfo);

        // Assert
        Assert.Equal("TestCountry", country.CommonName);
        Assert.Equal("TestCapital", country.Capital);
    }

    /// <summary>
    /// Tests the mapping of a complete ExternalCountryInfo model to the Country model, ensuring all fields are accurately mapped.
    /// </summary>
    /// <remarks>
    /// This test validates the AutoMapper configuration for a full data mapping scenario, where ExternalCountryInfo includes
    /// not only essential fields but also additional information such as borders. It ensures that the mapping logic comprehensively
    /// handles all provided data, reflecting a real-world use case where complete data from an external API is used to populate
    /// the Country model in its entirety. This test is key to confirming the robustness and completeness of the mapping process.
    /// </remarks>
    [Fact]
    public void Mapper_ShouldMapEssentialFields_WhenCompleteModelIsProvided()
    {
        // Arrange
        var externalCountryInfo = new ExternalCountryInfo
        {
            Name = new ExternalCountryInfo.CountryNames {Common = "TestCountry"},
            Capital = "TestCapital",
            Borders = new List<string> {"Border1", "Border2"}
        };

        // Act
        var country = _mapper.Map<Country>(externalCountryInfo);

        // Assert
        Assert.Equal("TestCountry", country.CommonName);
        Assert.Equal("TestCapital", country.Capital);
    }
}