using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image history")]
public record DockerImageHistoryOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("human")]
    public bool? Human { get; set; }

    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}