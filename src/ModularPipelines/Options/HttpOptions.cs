namespace ModularPipelines.Options;

public record HttpOptions(HttpRequestMessage HttpRequestMessage)
{
    public HttpClient? HttpClient { get; init; }
    public bool LogRequest { get; init; } = true;
    public bool LogResponse { get; init; } = true;

    public static implicit operator HttpOptions(HttpRequestMessage requestMessage) => new HttpOptions(requestMessage);
}