using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust revoke")]
[ExcludeFromCodeCoverage]
public record DockerTrustRevokeOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Image { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}