using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageHistoryOptions : DockerOptions
{
    public DockerImageHistoryOptions(
        string image
    )
    {
        CommandParts = ["image", "history"];

        Image = image;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Image { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--human")]
    public virtual bool? Human { get; set; }

    [CliFlag("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
