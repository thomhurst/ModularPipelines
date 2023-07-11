using ModularPipelines.Attributes;

namespace ModularPipelines.NuGet.Options;

[CommandPrecedingArguments("nuget", "push")]
public record NuGetUploadOptions
(
    IEnumerable<string> PackagePaths,

    [property: CommandSwitch("-s")]
    Uri FeedUri
) : NuGetOptions
{
    [CommandSwitch("-k")]
    [SecretValue]
    public string? ApiKey { get; init; }

    [BooleanCommandSwitch("-n")]
    public bool? NoSymbols { get; init; }
}
