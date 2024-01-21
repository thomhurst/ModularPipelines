using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container")]
[ExcludeFromCodeCoverage]
public record DockerContainerOptions : DockerOptions
{
}
