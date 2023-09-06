using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume prune")]
[ExcludeFromCodeCoverage]
public record DockerVolumePruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}
