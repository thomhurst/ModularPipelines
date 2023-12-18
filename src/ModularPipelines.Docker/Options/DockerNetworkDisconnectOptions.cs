using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network disconnect")]
[ExcludeFromCodeCoverage]
public record DockerNetworkDisconnectOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Network, [property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}