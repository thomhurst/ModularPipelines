using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "cache")]
[ExcludeFromCodeCoverage]
public record DockerScoutCacheOptions : DockerOptions
{
}
