using ModularPipelines.Http;

namespace ModularPipelines.Options;

public record HttpOptions(HttpRequestMessage HttpRequestMessage)
{
    public HttpClient? HttpClient { get; init; }
    public HttpLoggingType LoggingType { get; init; } = HttpLoggingType.RequestAndResponse;

    public static implicit operator HttpOptions(HttpRequestMessage requestMessage) => new HttpOptions(requestMessage);
}