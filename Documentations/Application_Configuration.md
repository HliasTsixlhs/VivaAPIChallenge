
# Application Configuration for Viva.Geo.API

This document provides detailed information on the configuration settings for the Viva.Geo.API. It includes descriptions of each setting, their default values, and guidelines for customization.

## Configuration Settings

### Serilog Settings

Serilog is used for logging in the Viva.Geo.API. The following settings configure its behavior and output format.

| Setting                          | Description                                                           | Default Value                                     |
|----------------------------------|-----------------------------------------------------------------------|---------------------------------------------------|
| `Serilog:Using`                  | Specifies the sinks Serilog will write to                             | `["Serilog.Sinks.Console", "Serilog.Sinks.Debug"]`|
| `Serilog:MinimumLevel:Default`   | Default minimum level for logging                                     | `Information`                                     |
| `Serilog:MinimumLevel:Override`  | Overrides the minimum level for specific namespaces                   | `{ "Microsoft": "Warning", "System": "Warning", "System.Net.Http.HttpClient.health-checks": "Warning" }`|
| `Serilog:WriteTo`                | Defines the output sinks for logging                                  | See detailed notes                                |
| `Serilog:Enrich`                 | List of properties to enrich the log entries                          | `["FromLogContext", "WithMachineName", "WithThreadId"]` |
| `Serilog:Properties:Application` | Additional properties for log context                                 | `VivaGeoAPI`                                      |

#### Detailed Notes for Serilog Settings

##### `Serilog:WriteTo`
- **Console Sink**: Logs to the console with a specific output template.
  - **Default Template**: `{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] (EventId: {EventId}) {Message}{NewLine}{Exception}`
- **Debug Sink**: Logs to the debug output with a detailed output template.
  - **Default Template**: `{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] (EventId: {EventId}) (CorrelationId: {CorrelationId}) (ReqId: {RequestId}) (ConnId: {ConnectionId}) (ConnRemotePort: {ConnectionRemotePort}) {Message}{NewLine}{Exception}`

### HealthCheckUI Settings

HealthCheckUI provides a UI for displaying the health of the API and its dependencies.

| Setting                                     | Description                                            | Default Value                       |
|---------------------------------------------|--------------------------------------------------------|-------------------------------------|
| `HealthCheckUI:EvaluationTimeInSeconds`     | Time in seconds between health checks                  | `10`                                |
| `HealthCheckUI:MaximumHistoryEntriesPerEndpoint` | Maximum history entries per endpoint                | `50`                                |
| `HealthCheckUI:ApiMaxActiveRequests`        | Maximum number of concurrent API requests              | `3`                                 |
| `HealthCheckUI:MinimumSecondsBetweenFailureNotifications` | Minimum time between failure notifications      | `60`                                |

### MemoryCacheOptions

Configuration for the in-memory cache used by the API.

| Setting                                     | Description                                       | Default Value  |
|---------------------------------------------|---------------------------------------------------|----------------|
| `MemoryCacheOptions:SizeLimit`              | Maximum size of the memory cache                  | `null`         |
| `MemoryCacheOptions:ExpirationScanFrequency` | Frequency of expiration scans                     | `00:01:00`     |
| `MemoryCacheOptions:CompactionPercentage`   | Percentage of cache to compact                    | `0.05`         |
| `MemoryCacheOptions:TrackLinkedCacheEntries`| Whether to track linked cache entries             | `false`        |
| `MemoryCacheOptions:TrackStatistics`        | Whether to track cache statistics                 | `false`        |

### DatabaseOptions

Settings related to the database connection used by the API.

| Setting                           | Description                      | Default Value                                                                                                |
|-----------------------------------|----------------------------------|--------------------------------------------------------------------------------------------------------------|
| `DatabaseOptions:ConnectionString`| Connection string for the database | `Server=sqlserver,1433;Database=GeoDatabase;User ID=sa;Password=Str@ngP@ssword;TrustServerCertificate=True;` |

### AllowedHosts

Defines the hosts allowed to access the API.

| Setting       | Description             | Default Value |
|---------------|-------------------------|---------------|
| `AllowedHosts`| List of allowed hosts   | `*`           |

## Options Pattern for Configuration

In Viva.Geo.API, we use the options pattern for setting up configuration. This involves creating options models for each configuration section and then binding these models to the corresponding sections in the configuration file. This approach enhances the maintainability and scalability of the application configuration.

### Example: HealthCheckUI Options Model

Here is an example of how we define and use an options model for the HealthCheckUI settings:

```csharp
namespace Common.HealthChecks.Options;

public class HealthCheckUIOptions
{
    public int EvaluationTimeInSeconds { get; set; }
    public int MaximumHistoryEntriesPerEndpoint { get; set; }
    public int ApiMaxActiveRequests { get; set; }
    public int MinimumSecondsBetweenFailureNotifications { get; set; }
}

var healthCheckUIOptions = new HealthCheckUIOptions();
configuration.Bind("HealthCheckUI", healthCheckUIOptions);
```

In this example, the `HealthCheckUIOptions` class represents the settings under the `HealthCheckUI` section of the configuration. We then create an instance of this class and bind it to the `HealthCheckUI` section in the configuration file using the `configuration.Bind` method. This pattern is followed for other configuration sections as well.
