using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose exec")]
public record DockerComposeExecOptions : DockerOptions
{
    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("env")]
    public string? Env { get; set; }

    [CommandLongSwitch("index")]
    public string? Index { get; set; }

    [BooleanCommandSwitch("no-TTY")]
    public bool? NoTTY { get; set; }

    [CommandLongSwitch("privileged")]
    public string? Privileged { get; set; }

    [CommandLongSwitch("user")]
    public string? User { get; set; }

    [CommandLongSwitch("workdir")]
    public string? Workdir { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}