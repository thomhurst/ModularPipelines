using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service ls")]
public record DockerServiceLsOptions : DockerOptions
{
    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}