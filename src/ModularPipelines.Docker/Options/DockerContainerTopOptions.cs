using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "top")]
[ExcludeFromCodeCoverage]
public record DockerContainerTopOptions : DockerOptions
{
}
