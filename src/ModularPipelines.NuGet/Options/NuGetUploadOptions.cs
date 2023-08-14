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
    public NuGetUploadOptions(string packagePath, Uri feedUri) : this( new []{ packagePath }, feedUri)
    {
    }

    [CommandSwitch("-k")]
    [SecretValue]
    public string? ApiKey { get; init; }

    [BooleanCommandSwitch("-n")]
    public bool? NoSymbols { get; init; }
}
