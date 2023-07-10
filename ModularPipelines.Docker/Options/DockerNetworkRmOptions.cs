using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network rm")]
public record DockerNetworkRmOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}