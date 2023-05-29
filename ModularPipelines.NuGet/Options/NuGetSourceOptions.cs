namespace ModularPipelines.NuGet.Options;

public record NuGetSourceOptions
(
    Uri FeedUri,
    string Name
)
{
    public string? Username { get; init; }
    public string? Password { get; init; }
}