using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "unpause")]
[ExcludeFromCodeCoverage]
public record DockerContainerUnpauseOptions : DockerOptions
{
}
