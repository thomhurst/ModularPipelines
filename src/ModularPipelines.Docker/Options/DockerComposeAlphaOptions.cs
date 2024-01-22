using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaOptions : DockerOptions
{
}
