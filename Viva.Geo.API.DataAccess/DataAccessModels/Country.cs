using System.ComponentModel.DataAnnotations;

namespace Viva.Geo.API.DataAccess.DataAccessModels;

public class Country
{
    public int CountryId { get; set; }
    [MaxLength(100)] public string CommonName { get; set; }
    [MaxLength(100)] public string Capital { get; set; }

    // Navigation property for the many-to-many relationship
    public ICollection<CountryBorder> CountryBorders { get; set; }
}