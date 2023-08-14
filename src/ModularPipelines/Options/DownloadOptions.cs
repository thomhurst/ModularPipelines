namespace ModularPipelines.Options;

public record DownloadOptions(Uri DownloadUri)
{
    public HttpClient? HttpClient { get; init; }
    public Action<HttpRequestMessage>? RequestConfigurator { get; init; }
}
