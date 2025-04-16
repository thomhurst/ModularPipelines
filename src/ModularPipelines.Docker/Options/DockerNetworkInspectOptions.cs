using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNetworkInspectOptions : DockerOptions
{
    public DockerNetworkInspectOptions(
        IEnumerable<string> network
    )
    {
        CommandParts = ["network", "inspect"];

        Network = network;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Network { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--verbose")]
    public virtual string? Verbose { get; set; }
}
