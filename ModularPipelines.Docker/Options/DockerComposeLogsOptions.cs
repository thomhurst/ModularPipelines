using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose logs")]
public record DockerComposeLogsOptions : DockerOptions
{
    [CommandLongSwitch("follow")]
    public string? Follow { get; set; }

    [CommandLongSwitch("no-color")]
    public string? NoColor { get; set; }

    [CommandLongSwitch("no-log-prefix")]
    public string? NoLogPrefix { get; set; }

    [CommandLongSwitch("since")]
    public string? Since { get; set; }

    [CommandLongSwitch("tail")]
    public string? Tail { get; set; }

    [CommandLongSwitch("timestamps")]
    public string? Timestamps { get; set; }

    [CommandLongSwitch("until")]
    public string? Until { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}