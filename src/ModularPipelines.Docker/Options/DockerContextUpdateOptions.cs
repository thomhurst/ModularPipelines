using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context update")]
public record DockerContextUpdateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Context) : DockerOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--docker")]
    public string? Docker { get; set; }
}