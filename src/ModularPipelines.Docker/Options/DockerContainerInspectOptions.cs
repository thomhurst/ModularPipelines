using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerInspectOptions : DockerOptions
{
    public DockerContainerInspectOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "inspect"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}
