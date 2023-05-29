namespace ModularPipelines.NuGet.Options;

public record NuGetUploadOptions
(
    IEnumerable<string> PackagePaths,
    Uri FeedUri
)
{
    public string? ApiKey { get; init; }
}