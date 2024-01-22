using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("builder")]
[ExcludeFromCodeCoverage]
public record DockerBuilderOptions : DockerOptions
{
}
