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
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A formatted string representation of the response.</returns>
    Task<string> FormatAsync(HttpResponseMessage response, CancellationToken cancellationToken = default);

    /// <summary>
    /// Formats an HTTP response message as a string with logging options.
    /// </summary>
    /// <param name="response">The HTTP response to format.</param>
    /// <param name="options">Options controlling what parts of the response to include.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A formatted string representation of the response.</returns>
    Task<string> FormatAsync(HttpResponseMessage response, HttpLoggingOptions options, CancellationToken cancellationToken = default);
}
