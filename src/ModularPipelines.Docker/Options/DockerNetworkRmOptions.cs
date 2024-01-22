using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network", "rm")]
[ExcludeFromCodeCoverage]
public record DockerNetworkRmOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Network { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
