using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaOptions : DockerOptions
{
}
