using ModularPipelines.Options;

namespace ModularPipelines.Http;

public interface IHttp
{
    Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default);
    HttpClient HttpClient { get; }
    HttpClient GetLoggingHttpClient(HttpLoggingType loggingType);
}
