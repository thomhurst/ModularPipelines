using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "rm")]
[ExcludeFromCodeCoverage]
public record DockerBuildxRmOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [CommandSwitch("--all-inactive")]
    public virtual string? AllInactive { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--keep-daemon")]
    public virtual string? KeepDaemon { get; set; }

    [CommandSwitch("--keep-state")]
    public virtual string? KeepState { get; set; }
}
