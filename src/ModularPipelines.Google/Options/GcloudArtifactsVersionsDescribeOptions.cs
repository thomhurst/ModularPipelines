using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "versions", "describe")]
public record GcloudArtifactsVersionsDescribeOptions : GcloudOptions
{
    public GcloudArtifactsVersionsDescribeOptions(
        string version,
        string location,
        string package,
        string repository
    )
    {
        GcloudArtifactsVersionsDescribeOptionsVersion = version;
        Location = location;
        Package = package;
        Repository = repository;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudArtifactsVersionsDescribeOptionsVersion { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Package { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Repository { get; set; }
}
