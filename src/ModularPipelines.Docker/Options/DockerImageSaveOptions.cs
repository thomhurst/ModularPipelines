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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Image { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }
}
