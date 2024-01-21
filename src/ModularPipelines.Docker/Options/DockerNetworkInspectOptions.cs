using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public string? Format { get; set; }

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }
}
