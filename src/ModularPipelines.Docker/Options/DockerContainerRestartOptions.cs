using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public string? Signal { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }
}
