using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose ls")]
[ExcludeFromCodeCoverage]
public record DockerComposeLsOptions : DockerOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
