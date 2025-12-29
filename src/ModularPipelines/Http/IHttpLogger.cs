using System.Net;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Provides functionality for logging HTTP requests and responses.
/// </summary>
public interface IHttpLogger
{
    /// <summary>
    /// Prints the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(HttpRequestMessage request, IModuleLogger logger);

    /// <summary>
    /// Prints the HTTP request with logging options.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the request to log.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(HttpRequestMessage request, IModuleLogger logger, HttpLoggingOptions options);

    /// <summary>
    /// Prints the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(HttpResponseMessage response, IModuleLogger logger);

    /// <summary>
    /// Prints the HTTP response with logging options.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the response to log.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(HttpResponseMessage response, IModuleLogger logger, HttpLoggingOptions options);

    /// <summary>
    /// Prints the HTTP status code.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code to print.</param>
    /// <param name="logger">The current module logger.</param>
    void PrintStatusCode(HttpStatusCode? httpStatusCode, IModuleLogger logger);

    /// <summary>
    /// Prints the duration of the HTTP request.
    /// </summary>
    /// <param name="duration">The duration to print.</param>
    /// <param name="logger">The current module logger.</param>
    void PrintDuration(TimeSpan duration, IModuleLogger logger);
}
