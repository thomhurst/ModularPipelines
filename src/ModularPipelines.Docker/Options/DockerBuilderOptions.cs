using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("builder")]
[ExcludeFromCodeCoverage]
public record DockerBuilderOptions : DockerOptions
{
}
