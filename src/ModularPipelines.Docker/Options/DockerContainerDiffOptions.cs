using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "diff")]
[ExcludeFromCodeCoverage]
public record DockerContainerDiffOptions : DockerOptions
{
}
