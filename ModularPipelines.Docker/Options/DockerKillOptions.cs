using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("kill")]
public record DockerKillOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
{

    [CommandSwitch("--signal")]
    public string? Signal { get; set; }

}