using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system")]
[ExcludeFromCodeCoverage]
public record DockerSystemOptions : DockerOptions
{
}
