using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageRmOptions : DockerOptions
{
    public DockerImageRmOptions(
        IEnumerable<string> image
    )
    {
        CommandParts = ["image", "rm"];

        Image = image;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Image { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--no-prune")]
    public virtual bool? NoPrune { get; set; }
}
