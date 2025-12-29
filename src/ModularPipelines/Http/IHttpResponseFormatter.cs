using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Provides functionality for formatting HTTP responses as strings.
/// </summary>
internal interface IHttpResponseFormatter
{
    /// <summary>
    /// Formats an HTTP response message as a string.
    /// </summary>
    /// <param name="response">The HTTP response to format.</param>
    /// <returns>A formatted string representation of the response.</returns>
    Task<string> FormatAsync(HttpResponseMessage response);

    /// <summary>
    /// Formats an HTTP response message as a string with logging options.
    /// </summary>
    /// <param name="response">The HTTP response to format.</param>
    /// <param name="options">Options controlling what parts of the response to include.</param>
    /// <returns>A formatted string representation of the response.</returns>
    Task<string> FormatAsync(HttpResponseMessage response, HttpLoggingOptions options);
}
