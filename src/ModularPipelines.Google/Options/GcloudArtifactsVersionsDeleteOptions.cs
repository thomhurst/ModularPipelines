using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "versions", "delete")]
public record GcloudArtifactsVersionsDeleteOptions : GcloudOptions
{
    public GcloudArtifactsVersionsDeleteOptions(
        string version,
        string location,
        string package,
        string repository
    )
    {
        GcloudArtifactsVersionsDeleteOptionsVersion = version;
        Location = location;
        Package = package;
        Repository = repository;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudArtifactsVersionsDeleteOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Package { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Repository { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--delete-tags")]
    public bool? DeleteTags { get; set; }
}
