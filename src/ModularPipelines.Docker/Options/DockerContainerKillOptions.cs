using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerKillOptions : DockerOptions
{
    public DockerContainerKillOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "kill"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }

    [CommandSwitch("--signal")]
    public string? Signal { get; set; }
}
