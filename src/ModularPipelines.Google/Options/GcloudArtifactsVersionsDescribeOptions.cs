using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "versions", "describe")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudArtifactsVersionsDescribeOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Package { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Repository { get; set; }
}
