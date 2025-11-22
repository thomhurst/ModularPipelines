namespace ModularPipelines.Http;

/// <summary>
/// Provides functionality for formatting HTTP requests as strings.
/// </summary>
internal interface IHttpRequestFormatter
{
    /// <summary>
    /// Formats an HTTP request message as a string.
    /// </summary>
    /// <param name="request">The HTTP request to format.</param>
    /// <returns>A formatted string representation of the request.</returns>
    Task<string> FormatAsync(HttpRequestMessage request);
}
