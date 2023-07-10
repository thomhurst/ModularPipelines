using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack services")]
public record DockerStackServicesOptions : DockerOptions
{
    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}