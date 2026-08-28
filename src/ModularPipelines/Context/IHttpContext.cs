using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IHttpContext
{
    /// <summary>
    /// Sends a HTTP request.
    /// </summary>
    /// <param name="httpOptions">Options to control logging and the client.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default);
}
