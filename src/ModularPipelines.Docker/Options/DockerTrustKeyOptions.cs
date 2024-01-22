using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "key")]
[ExcludeFromCodeCoverage]
public record DockerTrustKeyOptions : DockerOptions
{
}
