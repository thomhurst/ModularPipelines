using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context export")]
public record DockerContextExportOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Context) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? File { get; set; }
}