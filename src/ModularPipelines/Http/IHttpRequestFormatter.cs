using ModularPipelines.Options;

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
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A formatted string representation of the request.</returns>
    Task<string> FormatAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Formats an HTTP request message as a string with logging options.
    /// </summary>
    /// <param name="request">The HTTP request to format.</param>
    /// <param name="options">Options controlling what parts of the request to include.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A formatted string representation of the request.</returns>
    Task<string> FormatAsync(HttpRequestMessage request, HttpLoggingOptions options, CancellationToken cancellationToken = default);
}
