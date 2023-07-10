using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin inspect")]
public record DockerPluginInspectOptions : DockerOptions
{
}