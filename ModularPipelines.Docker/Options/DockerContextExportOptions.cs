using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context export")]
public record DockerContextExportOptions : DockerOptions
{
}