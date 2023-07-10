using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose stop")]
public record DockerComposeStopOptions : DockerOptions
{
    [CommandLongSwitch("timeout")]
    public string? Timeout { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}