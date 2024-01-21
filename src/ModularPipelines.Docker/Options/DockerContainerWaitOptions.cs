using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "wait")]
[ExcludeFromCodeCoverage]
public record DockerContainerWaitOptions : DockerOptions
{
}
