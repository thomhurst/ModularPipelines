using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container kill")]
public record DockerContainerKillOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
{
    [CommandSwitch("--signal")]
    public string? Signal { get; set; }
}