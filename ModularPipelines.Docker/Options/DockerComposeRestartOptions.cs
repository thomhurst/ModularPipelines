using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose restart")]
public record DockerComposeRestartOptions : DockerOptions
{
    [CommandLongSwitch("no-deps")]
    public string? NoDeps { get; set; }

    [CommandLongSwitch("timeout")]
    public string? Timeout { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}