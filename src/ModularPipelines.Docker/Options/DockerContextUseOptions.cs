using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context", "use")]
[ExcludeFromCodeCoverage]
public record DockerContextUseOptions : DockerOptions
{
}
