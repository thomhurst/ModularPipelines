using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "start")]
[ExcludeFromCodeCoverage]
public record DockerComposeStartOptions : DockerOptions
{
}
