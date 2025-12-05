using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "versions", "delete")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudArtifactsVersionsDeleteOptionsVersion { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Package { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Repository { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--delete-tags")]
    public bool? DeleteTags { get; set; }
}
