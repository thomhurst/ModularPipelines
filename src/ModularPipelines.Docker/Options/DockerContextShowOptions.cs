using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context", "show")]
[ExcludeFromCodeCoverage]
public record DockerContextShowOptions : DockerOptions
{
}
