using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "cache", "df")]
[ExcludeFromCodeCoverage]
public record DockerScoutCacheDfOptions : DockerOptions
{
}
