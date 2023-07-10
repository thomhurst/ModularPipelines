using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system info")]
public record DockerSystemInfoOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

}