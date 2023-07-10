using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust inspect")]
public record DockerTrustInspectOptions : DockerOptions
{
    [CommandLongSwitch("pretty")]
    public string? Pretty { get; set; }

}