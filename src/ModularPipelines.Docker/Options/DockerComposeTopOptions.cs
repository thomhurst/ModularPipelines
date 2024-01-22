using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "top")]
[ExcludeFromCodeCoverage]
public record DockerComposeTopOptions : DockerOptions
{
}
