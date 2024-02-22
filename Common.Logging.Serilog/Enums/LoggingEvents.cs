namespace Common.Logging.Serilog.Enums;

// Note: Please ensure these settings are updated in accordance with the evolving business logic of the application.
public enum VivaGeoApiEvent
{
    CountryProcessing = 1000, // Event for country data retrieval
    BorderProcessing,        // Event for processing border data
    ArrayProcessing          // Event for array processing tasks
}
