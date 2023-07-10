using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config ls")]
public record DockerConfigLsOptions : DockerOptions
{
    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}