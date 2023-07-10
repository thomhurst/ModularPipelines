using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context inspect")]
public record DockerContextInspectOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

}