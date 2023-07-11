using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container export")]
public record DockerContainerExportOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [CommandSwitch("--output")]
    public string? Output { get; set; }

}