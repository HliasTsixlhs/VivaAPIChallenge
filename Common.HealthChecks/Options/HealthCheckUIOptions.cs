namespace Common.HealthChecks.Options;

public class HealthCheckUIOptions
{
    // Defines the interval in seconds for evaluating health checks.
    // The UI will poll health checks based on this time.
    public int EvaluationTimeInSeconds { get; set; }

    // Sets the maximum number of execution history records per endpoint.
    // Helps in maintaining the size of the stored history.
    public int MaximumHistoryEntriesPerEndpoint { get; set; }

    // Limits the maximum number of concurrent requests to the health check API.
    // Helps in controlling the load on the health check endpoint.
    public int ApiMaxActiveRequests { get; set; }

    // Determines the minimum duration in seconds between consecutive notifications for a failure.
    // Useful to prevent too frequent notifications in case of failing health checks.
    public int MinimumSecondsBetweenFailureNotifications { get; set; }
}