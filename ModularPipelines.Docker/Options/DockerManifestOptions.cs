using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest")]
public record DockerManifestOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}