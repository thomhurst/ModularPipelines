using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "cache", "prune")]
[ExcludeFromCodeCoverage]
public record DockerScoutCachePruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--sboms")]
    public string? Sboms { get; set; }
}
