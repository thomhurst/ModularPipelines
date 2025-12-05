using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerCpOptions : DockerOptions
{
    public DockerContainerCpOptions(
        string container,
        string destPath,
        string cp,
        string srcPath
    )
    {
        CommandParts = ["container", "cp"];

        Container = container;

        DestPath = destPath;

        Cp = cp;

        SrcPath = srcPath;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Container { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? DestPath { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Cp { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Options { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? SrcPath { get; set; }

    [CliOption("--archive")]
    public virtual string? Archive { get; set; }

    [CliOption("--follow-link")]
    public virtual string? FollowLink { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
