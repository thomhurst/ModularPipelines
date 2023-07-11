using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("export")]
public record DockerExportOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{

    [CommandSwitch("--output")]
    public string? Output { get; set; }
}