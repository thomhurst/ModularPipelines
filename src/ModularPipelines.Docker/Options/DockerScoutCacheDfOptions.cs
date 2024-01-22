using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "cache", "df")]
[ExcludeFromCodeCoverage]
public record DockerScoutCacheDfOptions : DockerOptions
{
}
