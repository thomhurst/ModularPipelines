using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service scale")]
public record DockerServiceScaleOptions : DockerOptions
{
    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

}