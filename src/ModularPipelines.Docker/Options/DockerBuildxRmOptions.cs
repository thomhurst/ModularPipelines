using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx rm")]
[ExcludeFromCodeCoverage]
public record DockerBuildxRmOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [CommandSwitch("--all-inactive")]
    public string? AllInactive { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--keep-daemon")]
    public string? KeepDaemon { get; set; }

    [CommandSwitch("--keep-state")]
    public string? KeepState { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}