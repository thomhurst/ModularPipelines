using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network inspect")]
public record DockerNetworkInspectOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("verbose")]
    public string? Verbose { get; set; }

}