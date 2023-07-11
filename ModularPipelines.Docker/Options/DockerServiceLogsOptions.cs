using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service logs")]
public record DockerServiceLogsOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Service) : DockerOptions
{
    [BooleanCommandSwitch("--details")]
    public bool? Details { get; set; }

    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [BooleanCommandSwitch("--no-resolve")]
    public bool? NoResolve { get; set; }

    [BooleanCommandSwitch("--no-task-ids")]
    public bool? NoTaskIds { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--raw")]
    public bool? Raw { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [CommandSwitch("--tail")]
    public string? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public bool? Timestamps { get; set; }

}