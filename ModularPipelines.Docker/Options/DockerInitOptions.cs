using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("init")]
public record DockerInitOptions : DockerOptions
{
    [CommandLongSwitch("version")]
    public string? Version { get; set; }

}