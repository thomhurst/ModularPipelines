using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust key load")]
public record DockerTrustKeyLoadOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Keyfile) : DockerOptions
{

    [CommandSwitch("--name")]
    public string? Name { get; set; }

}