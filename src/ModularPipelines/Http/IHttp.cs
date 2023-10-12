using ModularPipelines.Options;

namespace ModularPipelines.Http;

public interface IHttp
{
    /// <summary>
    /// Sends a HTTP request.
    /// </summary>
    /// <param name="httpOptions">Options to control logging and the client.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the default, non-logging HTTP client.
    /// </summary>
    HttpClient HttpClient { get; }

    /// <summary>
    /// Gets a logging HTTP client.
    /// </summary>
    /// <returns>A logging HTTP client.</returns>
    HttpClient GetLoggingHttpClient(HttpLoggingType loggingType = HttpLoggingType.Request | HttpLoggingType.Response | HttpLoggingType.StatusCode | HttpLoggingType.Duration);
}
