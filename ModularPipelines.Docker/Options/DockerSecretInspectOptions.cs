using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret inspect")]
public record DockerSecretInspectOptions : DockerOptions
{
    [CommandLongSwitch("pretty")]
    public string? Pretty { get; set; }

}