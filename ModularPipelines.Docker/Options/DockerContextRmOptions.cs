using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context rm")]
public record DockerContextRmOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}