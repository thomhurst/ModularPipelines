using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service logs")]
public record DockerServiceLogsOptions : DockerOptions
{
    [CommandLongSwitch("details")]
    public string? Details { get; set; }

    [CommandLongSwitch("follow")]
    public string? Follow { get; set; }

    [CommandLongSwitch("no-resolve")]
    public string? NoResolve { get; set; }

    [CommandLongSwitch("no-task-ids")]
    public string? NoTaskIds { get; set; }

    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

    [CommandLongSwitch("raw")]
    public string? Raw { get; set; }

    [CommandLongSwitch("since")]
    public string? Since { get; set; }

    [CommandLongSwitch("tail")]
    public string? Tail { get; set; }

    [CommandLongSwitch("timestamps")]
    public string? Timestamps { get; set; }

}