using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "pull")]
[ExcludeFromCodeCoverage]
public record DockerComposePullOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--ignore-buildable")]
    public virtual string? IgnoreBuildable { get; set; }

    [CommandSwitch("--ignore-pull-failures")]
    public virtual string? IgnorePullFailures { get; set; }

    [CommandSwitch("--include-deps")]
    public virtual string? IncludeDeps { get; set; }

    [BooleanCommandSwitch("--no-parallel")]
    public virtual bool? NoParallel { get; set; }

    [BooleanCommandSwitch("--parallel")]
    public virtual bool? Parallel { get; set; }

    [CommandSwitch("--policy")]
    public virtual string? Policy { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
