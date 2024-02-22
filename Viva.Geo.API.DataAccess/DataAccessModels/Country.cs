namespace Viva.Geo.API.DataAccess.DataAccessModels;

public class Country
{
    public int CountryId { get; set; }
    public string CommonName { get; set; }
    public string Capital { get; set; }

    // Navigation property for the many-to-many relationship
    public ICollection<CountryBorder> CountryBorders { get; set; }
}