using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("port")]
public record DockerPortOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? PrivatePort { get; set; }
}