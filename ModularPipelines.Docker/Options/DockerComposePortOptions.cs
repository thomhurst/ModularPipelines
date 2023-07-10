using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose port")]
public record DockerComposePortOptions : DockerOptions
{
    [CommandLongSwitch("index")]
    public string? Index { get; set; }

    [CommandLongSwitch("protocol")]
    public string? Protocol { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}