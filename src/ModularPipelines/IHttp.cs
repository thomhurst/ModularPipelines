using ModularPipelines.Options;

namespace ModularPipelines;

public interface IHttp
{
    Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions);
    HttpClient HttpClient { get; }
}
