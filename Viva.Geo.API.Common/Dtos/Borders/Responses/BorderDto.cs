namespace Viva.Geo.API.Common.Dtos.Borders.Responses;

public class BorderDto
{
    public int BorderId { get; set; }
    public string BorderCode { get; set; } // For example, "ALB", "BGR", etc.

    // Navigation property for the many-to-many relationship
    // public ICollection<CountryBorder> CountryBorders { get; set; }
}