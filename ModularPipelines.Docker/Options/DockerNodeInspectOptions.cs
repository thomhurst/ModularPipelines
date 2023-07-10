using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node inspect")]
public record DockerNodeInspectOptions : DockerOptions
{
    [CommandLongSwitch("pretty")]
    public string? Pretty { get; set; }

}