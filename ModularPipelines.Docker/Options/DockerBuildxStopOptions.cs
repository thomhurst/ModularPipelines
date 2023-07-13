using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx stop")]
public record DockerBuildxStopOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Name { get; set; }
}