using ModularPipelines.Options;

namespace ModularPipelines;

public interface IHttp
{
    public Task<HttpResponseMessage> Send(HttpOptions httpOptions);
}