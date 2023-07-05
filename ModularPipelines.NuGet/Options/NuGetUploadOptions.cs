using ModularPipelines.Attributes;

namespace ModularPipelines.NuGet.Options;

public record NuGetUploadOptions
(
    IEnumerable<string> PackagePaths,
    
    [property: CommandSwitch("s")]
    Uri FeedUri
)
{
    [property: CommandSwitch("k")]
    public string? ApiKey { get; init; }
}