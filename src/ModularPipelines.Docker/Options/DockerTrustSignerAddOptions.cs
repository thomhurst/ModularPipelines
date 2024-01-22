using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "signer", "add")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerAddOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Repository { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }
}
