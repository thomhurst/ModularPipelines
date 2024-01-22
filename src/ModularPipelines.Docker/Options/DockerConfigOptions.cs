using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config")]
[ExcludeFromCodeCoverage]
public record DockerConfigOptions : DockerOptions
{
}
