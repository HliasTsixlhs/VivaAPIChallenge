namespace Viva.Geo.API.DataAccess.DataAccessModels;

public class Border
{
    public int BorderId { get; set; }

    public string BorderCode { get; set; } // For example, "ALB", "BGR", etc.

    // Navigation property for the many-to-many relationship
    public ICollection<CountryBorder> CountryBorders { get; set; }
}