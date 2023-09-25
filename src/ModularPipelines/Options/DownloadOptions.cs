using ModularPipelines.Http;

namespace ModularPipelines.Options;

public record DownloadOptions(Uri DownloadUri)
{
    public HttpClient? HttpClient { get; init; }

    public Action<HttpRequestMessage>? RequestConfigurator { get; init; }

    public HttpLoggingType LoggingType { get; init; } = HttpLoggingType.RequestAndResponse;
}
