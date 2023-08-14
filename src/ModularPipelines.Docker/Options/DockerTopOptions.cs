using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("top")]
public record DockerTopOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions;