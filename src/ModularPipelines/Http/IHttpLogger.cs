using System.Net;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
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
    Task PrintRequest(HttpRequestMessage request, ILogger logger);

    /// <summary>
    /// Prints the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="cancellationToken">A token to cancel request formatting.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(
        HttpRequestMessage request,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        return WaitForLoggingAsync(PrintRequest(request, logger), cancellationToken);
    }

    /// <summary>
    /// Prints the HTTP request with logging options.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the request to log.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(HttpRequestMessage request, ILogger logger, HttpLoggingOptions options);

    /// <summary>
    /// Prints the HTTP request with logging options.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the request to log.</param>
    /// <param name="cancellationToken">A token to cancel request formatting.</param>
    /// <param name="baseAddress">The client base address used to identify webhook credentials.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintRequest(
        HttpRequestMessage request,
        ILogger logger,
        HttpLoggingOptions options,
        CancellationToken cancellationToken,
        Uri? baseAddress = null)
    {
        return WaitForLoggingAsync(PrintRequest(request, logger, options), cancellationToken);
    }

    /// <summary>
    /// Prints the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(HttpResponseMessage response, ILogger logger);

    /// <summary>
    /// Prints the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="cancellationToken">A token to cancel response formatting.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(
        HttpResponseMessage response,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        return WaitForLoggingAsync(PrintResponse(response, logger), cancellationToken);
    }

    /// <summary>
    /// Prints the HTTP response with logging options.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <param name="options">Options controlling what parts of the response to log.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintResponse(HttpResponseMessage response, ILogger logger, HttpLoggingOptions options);

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
        ILogger logger,
        HttpLoggingOptions options,
        CancellationToken cancellationToken)
    {
        return WaitForLoggingAsync(PrintResponse(response, logger, options), cancellationToken);
    }

    /// <summary>
    /// Prints the HTTP status code.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code to print.</param>
    /// <param name="logger">The current module logger.</param>
    void PrintStatusCode(HttpStatusCode? httpStatusCode, ILogger logger);

    /// <summary>
    /// Prints the duration of the HTTP request.
    /// </summary>
    /// <param name="duration">The duration to print.</param>
    /// <param name="logger">The current module logger.</param>
    void PrintDuration(TimeSpan duration, ILogger logger);

    private static Task WaitForLoggingAsync(Task loggingTask, CancellationToken cancellationToken)
    {
        // Cancellation stops waiting, but a legacy logger can still fault while buffering content.
        TaskObservation.ObserveFault(loggingTask);
        return loggingTask.WaitAsync(cancellationToken);
    }
}
