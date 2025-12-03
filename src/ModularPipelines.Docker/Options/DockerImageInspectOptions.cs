using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Image { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }
}
