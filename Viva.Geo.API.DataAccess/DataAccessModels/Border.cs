using System.ComponentModel.DataAnnotations;

namespace Viva.Geo.API.DataAccess.DataAccessModels;

public class Border
{
    public int BorderId { get; set; }
    [MaxLength(50)] public string BorderCode { get; set; }

    // Navigation property for the many-to-many relationship
    public ICollection<CountryBorder> CountryBorders { get; set; }
}