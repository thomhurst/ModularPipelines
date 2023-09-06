using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin ls")]
[ExcludeFromCodeCoverage]
public record DockerPluginLsOptions : DockerOptions
{
    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
