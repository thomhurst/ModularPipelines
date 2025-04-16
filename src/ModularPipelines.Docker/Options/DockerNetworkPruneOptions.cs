using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network", "prune")]
[ExcludeFromCodeCoverage]
public record DockerNetworkPruneOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }
}
