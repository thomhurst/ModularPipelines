using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("logs")]
public record DockerLogsOptions : DockerOptions
{
    [CommandLongSwitch("details")]
    public string? Details { get; set; }

    [CommandLongSwitch("follow")]
    public string? Follow { get; set; }

    [CommandLongSwitch("since")]
    public string? Since { get; set; }

    [CommandLongSwitch("tail")]
    public string? Tail { get; set; }

    [CommandLongSwitch("timestamps")]
    public string? Timestamps { get; set; }

}