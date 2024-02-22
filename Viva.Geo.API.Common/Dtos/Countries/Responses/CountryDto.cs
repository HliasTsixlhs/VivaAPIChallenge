namespace Viva.Geo.API.Common.Dtos.Countries.Responses;

public class CountryDto
{
    public string CommonName { get; set; }
    public string Capital { get; set; }
    public List<string> Borders { get; set; }
}