using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "prune")]
[ExcludeFromCodeCoverage]
public record DockerBuildxPruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--keep-storage")]
    public string? KeepStorage { get; set; }

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }
}
