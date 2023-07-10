using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network disconnect")]
public record DockerNetworkDisconnectOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}