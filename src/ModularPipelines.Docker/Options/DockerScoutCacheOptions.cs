using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "cache")]
[ExcludeFromCodeCoverage]
public record DockerScoutCacheOptions : DockerOptions
{
}
