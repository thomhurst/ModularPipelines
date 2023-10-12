using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose build")]
[ExcludeFromCodeCoverage]
public record DockerComposeBuildOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--build-arg")]
    public IEnumerable<KeyValue>? BuildArgs { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--push")]
    public bool? Push { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--ssh")]
    public string? Ssh { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
