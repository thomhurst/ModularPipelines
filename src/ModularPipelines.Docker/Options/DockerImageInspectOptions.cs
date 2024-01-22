using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageInspectOptions : DockerOptions
{
    public DockerImageInspectOptions(
        IEnumerable<string> image
    )
    {
        CommandParts = ["image", "inspect"];

        Image = image;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Image { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
