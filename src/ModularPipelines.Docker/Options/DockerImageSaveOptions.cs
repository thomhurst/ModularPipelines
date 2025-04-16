using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageSaveOptions : DockerOptions
{
    public DockerImageSaveOptions(
        IEnumerable<string> image
    )
    {
        CommandParts = ["image", "save"];

        Image = image;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Image { get; set; }

    [CommandSwitch("--output")]
    public virtual string? Output { get; set; }
}
