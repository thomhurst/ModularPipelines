using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config inspect")]
public record DockerConfigInspectOptions : DockerOptions
{
    [CommandLongSwitch("pretty")]
    public string? Pretty { get; set; }

}