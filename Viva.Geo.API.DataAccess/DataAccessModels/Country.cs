namespace Viva.Geo.API.DataAccess.DataAccessModels;

public class Country
{
    public int CountryId { get; set; }
    public string CommonName { get; set; }
    public string Capital { get; set; }

    public ICollection<Border> Borders { get; set; } // Collection of Border, not Country
}