using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stats")]
public record DockerStatsOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("no-stream")]
    public string? NoStream { get; set; }

    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

}