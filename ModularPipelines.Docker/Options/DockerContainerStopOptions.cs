using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container stop")]
public record DockerContainerStopOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
{
    [CommandSwitch("--signal")]
    public string? Signal { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }

}