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
    public IEnumerable<KeyValue>? BuildArg { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

    [BooleanCommandSwitch("--compress")]
    public bool? Compress { get; set; }

    [BooleanCommandSwitch("--force-rm")]
    public bool? ForceRm { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [CommandSwitch("--no-rm")]
    public string? NoRm { get; set; }

    [BooleanCommandSwitch("--parallel")]
    public bool? Parallel { get; set; }

    [CommandSwitch("--progress")]
    public string? Progress { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--push")]
    public bool? Push { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--ssh")]
    public string? Ssh { get; set; }

    [CommandSwitch("--with-dependencies")]
    public string? WithDependencies { get; set; }
}
