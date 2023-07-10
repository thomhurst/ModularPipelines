using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container stats")]
public record DockerContainerStatsOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("no-stream")]
    public string? NoStream { get; set; }

    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

}