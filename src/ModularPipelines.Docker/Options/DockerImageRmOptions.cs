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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Image { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--no-prune")]
    public virtual bool? NoPrune { get; set; }
}
