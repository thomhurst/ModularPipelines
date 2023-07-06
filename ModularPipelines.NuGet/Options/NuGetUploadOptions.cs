using ModularPipelines.Attributes;

namespace ModularPipelines.NuGet.Options;

[CommandPrecedingArguments("nuget", "push")]
public record NuGetUploadOptions
(
    IEnumerable<string> PackagePaths,

[property: CommandSwitch("s")]
    Uri FeedUri
)  : NuGetOptions
{
    [property: CommandSwitch("k")]
    public string? ApiKey { get; init; }
}