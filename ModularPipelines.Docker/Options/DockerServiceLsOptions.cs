using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service ls")]
public record DockerServiceLsOptions : DockerOptions
{
    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }


    [CommandSwitch("--filter")]
    public string? Filter { get; set; }


    [CommandSwitch("--format")]
    public string? Format { get; set; }

}