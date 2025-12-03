using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerTopOptions : DockerOptions
{
    public DockerContainerTopOptions(
        string options
    )
    {
        CommandParts = ["container", "top"];

        Options = options;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Ps { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Options { get; set; }
}
