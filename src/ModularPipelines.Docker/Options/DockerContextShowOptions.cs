using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context", "show")]
[ExcludeFromCodeCoverage]
public record DockerContextShowOptions : DockerOptions
{
}
