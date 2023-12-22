using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust signer add")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerAddOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Options, [property: PositionalArgument(Position = Position.AfterSwitches)] string Name, [property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Repositories) : DockerOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }
}