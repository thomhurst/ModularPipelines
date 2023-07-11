using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust signer add")]
public record DockerTrustSignerAddOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Options, [property: PositionalArgument(Position = Position.AfterArguments)] string Name, [property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Repositories) : DockerOptions
{

    [CommandSwitch("--key")]
    public string? Key { get; set; }
}