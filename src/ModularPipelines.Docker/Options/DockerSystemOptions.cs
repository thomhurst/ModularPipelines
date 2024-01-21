using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system")]
[ExcludeFromCodeCoverage]
public record DockerSystemOptions : DockerOptions
{
}
