using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container ls")]
public record DockerContainerLsOptions : DockerOptions
{
    [CommandLongSwitch("all")]
    public string? All { get; set; }

    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("last")]
    public string? Last { get; set; }

    [CommandLongSwitch("latest")]
    public string? Latest { get; set; }

    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("size")]
    public string? Size { get; set; }

}