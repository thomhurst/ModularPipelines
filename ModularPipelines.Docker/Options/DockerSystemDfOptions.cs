using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system df")]
public record DockerSystemDfOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("verbose")]
    public string? Verbose { get; set; }

}