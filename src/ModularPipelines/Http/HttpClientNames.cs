namespace ModularPipelines.Http;

/// <summary>
/// Helper class to generate consistent named HttpClient identifiers based on logging configuration.
/// </summary>
internal static class HttpClientNames
{
    private const string ClientNamePrefix = "ModularPipelines_LoggingClient_";

    /// <summary>
    /// Gets the named HttpClient identifier for the specified logging type.
    /// </summary>
    /// <param name="loggingType">The HTTP logging type flags.</param>
    /// <returns>A consistent client name for the logging configuration.</returns>
    public static string GetClientName(HttpLoggingType loggingType)
    {
        return $"{ClientNamePrefix}{(int)loggingType}";
    }
}
