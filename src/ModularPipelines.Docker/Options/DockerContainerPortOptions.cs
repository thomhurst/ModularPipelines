using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "port")]
[ExcludeFromCodeCoverage]
public record DockerContainerPortOptions : DockerOptions
{
}
