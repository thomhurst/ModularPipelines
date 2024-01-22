using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context")]
[ExcludeFromCodeCoverage]
public record DockerContextOptions : DockerOptions
{
}
