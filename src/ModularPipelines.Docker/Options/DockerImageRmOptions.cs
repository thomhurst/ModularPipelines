using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--no-prune")]
    public bool? NoPrune { get; set; }
}
