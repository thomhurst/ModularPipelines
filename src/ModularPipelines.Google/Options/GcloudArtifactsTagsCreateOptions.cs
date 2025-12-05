using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "tags", "create")]
public record GcloudArtifactsTagsCreateOptions : GcloudOptions
{
    public GcloudArtifactsTagsCreateOptions(
        string tag,
        string location,
        string package,
        string repository,
        string version
    )
    {
        Tag = tag;
        Location = location;
        Package = package;
        Repository = repository;
        Version = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Tag { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Package { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Repository { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }
}
