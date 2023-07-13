using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx rm")]
public record DockerBuildxRmOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
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