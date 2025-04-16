using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "build")]
[ExcludeFromCodeCoverage]
public record DockerComposeBuildOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--build-arg")]
    public virtual IEnumerable<KeyValue>? BuildArg { get; set; }

    [CommandSwitch("--builder")]
    public virtual string? Builder { get; set; }

    [BooleanCommandSwitch("--compress")]
    public virtual bool? Compress { get; set; }

    [BooleanCommandSwitch("--force-rm")]
    public virtual bool? ForceRm { get; set; }

    [CommandSwitch("--memory")]
    public virtual string? Memory { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CommandSwitch("--no-rm")]
    public virtual string? NoRm { get; set; }

    [BooleanCommandSwitch("--parallel")]
    public virtual bool? Parallel { get; set; }

    [CommandSwitch("--progress")]
    public virtual string? Progress { get; set; }

    [BooleanCommandSwitch("--pull")]
    public virtual bool? Pull { get; set; }

    [BooleanCommandSwitch("--push")]
    public virtual bool? Push { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandSwitch("--ssh")]
    public virtual string? Ssh { get; set; }

    [CommandSwitch("--with-dependencies")]
    public virtual string? WithDependencies { get; set; }
}
