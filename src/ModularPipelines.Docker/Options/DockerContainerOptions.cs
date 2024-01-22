using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container")]
[ExcludeFromCodeCoverage]
public record DockerContainerOptions : DockerOptions
{
}
