using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume ls")]
public record DockerVolumeLsOptions : DockerOptions
{
    [CommandLongSwitch("cluster")]
    public string? Cluster { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}