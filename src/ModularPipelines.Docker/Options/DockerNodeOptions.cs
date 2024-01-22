using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node")]
[ExcludeFromCodeCoverage]
public record DockerNodeOptions : DockerOptions
{
}
