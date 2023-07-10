using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume inspect")]
public record DockerVolumeInspectOptions : DockerOptions
{
}