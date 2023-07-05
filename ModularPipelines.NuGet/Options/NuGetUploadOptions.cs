using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.NuGet.Options;

public record NuGetUploadOptions
(
    IEnumerable<string> PackagePaths,
    
    [property: CommandSwitch("s")]
    Uri FeedUri
) : CommandLineOptions
{
    [property: CommandSwitch("k")]
    public string? ApiKey { get; init; }
}