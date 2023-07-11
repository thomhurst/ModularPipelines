using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container top")]
public record DockerContainerTopOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions;