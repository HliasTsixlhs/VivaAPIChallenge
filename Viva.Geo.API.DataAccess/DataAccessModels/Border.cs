using Viva.Geo.API.DataAccess.DataAccessModels;

public class Border
{
    public int CountryId { get; set; }
    public Country Country { get; set; }

    public int BorderCountryId { get; set; }
    public Country BorderCountry { get; set; }
}