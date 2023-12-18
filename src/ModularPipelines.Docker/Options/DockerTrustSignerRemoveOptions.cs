using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust signer remove")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerRemoveOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Name, [property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Repositories) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}