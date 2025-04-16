using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "cache", "prune")]
[ExcludeFromCodeCoverage]
public record DockerScoutCachePruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--sboms")]
    public virtual string? Sboms { get; set; }
}
