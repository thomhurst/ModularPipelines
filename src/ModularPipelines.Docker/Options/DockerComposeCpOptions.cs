using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerComposeCpOptions : DockerOptions
{
    public DockerComposeCpOptions(
        string service,
        string destPath,
        string compose,
        string cp,
        string srcPath
    )
    {
        CommandParts = ["compose", "cp"];

        Service = service;

        DestPath = destPath;

        Compose = compose;

        Cp = cp;

        SrcPath = srcPath;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Service { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? DestPath { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Compose { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Cp { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Options { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? SrcPath { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--archive")]
    public virtual string? Archive { get; set; }

    [CliOption("--follow-link")]
    public virtual string? FollowLink { get; set; }

    [CliOption("--index")]
    public virtual string? Index { get; set; }
}