namespace Viva.Geo.API.DataAccess.DataAccessModels;

public class CountryBorder
{
    public int CountryId { get; set; }
    public Country Country { get; set; }

    public int BorderId { get; set; }
    public Border Border { get; set; }
}