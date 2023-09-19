using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose pull")]
[ExcludeFromCodeCoverage]
public record DockerComposePullOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--ignore-buildable")]
    public string? IgnoreBuildable { get; set; }

    [CommandSwitch("--ignore-pull-failures")]
    public string? IgnorePullFailures { get; set; }

    [CommandSwitch("--include-deps")]
    public string? IncludeDeps { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
