using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual string? Format { get; set; }

    [CommandSwitch("--size")]
    public virtual string? Size { get; set; }
}
