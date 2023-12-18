using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose down")]
[ExcludeFromCodeCoverage]
public record DockerComposeDownOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Services { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public bool? RemoveOrphans { get; set; }

    [BooleanCommandSwitch("--rmi")]
    public bool? Rmi { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--volumes")]
    public string? Volumes { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}