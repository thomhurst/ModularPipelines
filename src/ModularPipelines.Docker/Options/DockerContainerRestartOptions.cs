using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerRestartOptions : DockerOptions
{
    public DockerContainerRestartOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "restart"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }

    [CommandSwitch("--signal")]
    public virtual string? Signal { get; set; }

    [CommandSwitch("--time")]
    public virtual string? Time { get; set; }
}
