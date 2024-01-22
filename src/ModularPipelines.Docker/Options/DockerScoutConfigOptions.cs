using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "config")]
[ExcludeFromCodeCoverage]
public record DockerScoutConfigOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Key { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Value { get; set; }
}
