using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerRmOptions : DockerOptions
{
    public DockerContainerRmOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "rm"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--link")]
    public string? Link { get; set; }

    [CommandSwitch("--volumes")]
    public string? Volumes { get; set; }
}
