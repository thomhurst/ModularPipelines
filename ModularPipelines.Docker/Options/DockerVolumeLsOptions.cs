using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume ls")]
public record DockerVolumeLsOptions : DockerOptions
{

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }


    [CommandSwitch("--filter")]
    public string? Filter { get; set; }


    [CommandSwitch("--format")]
    public string? Format { get; set; }

}