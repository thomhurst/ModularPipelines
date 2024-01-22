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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Ps { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Options { get; set; }
}
