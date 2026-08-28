using System.Net;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Provides functionality for logging HTTP requests and responses.
/// </summary>
internal interface IHttpLogger
{
    /// <summary>
    /// Prints the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(HttpRequestMessage request, IModuleLogger logger);

    /// <summary>
    /// Prints the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="cancellationToken">A token to cancel request formatting.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(
        HttpRequestMessage request,
        IModuleLogger logger,
        CancellationToken cancellationToken)
    {
        return PrintRequest(request, logger).WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Prints the HTTP request with logging options.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the request to log.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(HttpRequestMessage request, IModuleLogger logger, HttpLoggingOptions options);

    /// <summary>
    /// Prints the HTTP request with logging options.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the request to log.</param>
    /// <param name="cancellationToken">A token to cancel request formatting.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(
        HttpRequestMessage request,
        IModuleLogger logger,
        HttpLoggingOptions options,
        CancellationToken cancellationToken)
    {
        return PrintRequest(request, logger, options).WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Prints the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(HttpResponseMessage response, IModuleLogger logger);

    /// <summary>
    /// Prints the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="cancellationToken">A token to cancel response formatting.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(
        HttpResponseMessage response,
        IModuleLogger logger,
        CancellationToken cancellationToken)
    {
        return PrintResponse(response, logger).WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Prints the HTTP response with logging options.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the response to log.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(HttpResponseMessage response, IModuleLogger logger, HttpLoggingOptions options);

    /// <summary>
    /// Prints the HTTP response with logging options.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the response to log.</param>
    /// <param name="cancellationToken">A token to cancel response formatting.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(
        HttpResponseMessage response,
        IModuleLogger logger,
        HttpLoggingOptions options,
        CancellationToken cancellationToken)
    {
        return PrintResponse(response, logger, options).WaitAsync(cancellationToken);
    }

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
