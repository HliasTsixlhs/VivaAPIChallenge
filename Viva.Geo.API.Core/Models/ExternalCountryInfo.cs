namespace Viva.Geo.API.Core.Models;

public class ExternalCountryInfo
{
    public CountryNames Name { get; set; }
    public List<string> Capital { get; set; }
    public List<string> Borders { get; set; }

    public class CountryNames
    {
        public string Common { get; set; }
    }
}