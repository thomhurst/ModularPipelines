using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "prune")]
[ExcludeFromCodeCoverage]
public record DockerContainerPruneOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
