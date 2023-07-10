using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("exec")]
public record DockerExecOptions : DockerOptions
{
    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("detach-keys")]
    public string? DetachKeys { get; set; }

    [CommandLongSwitch("env-file")]
    public string? EnvFile { get; set; }

    [CommandLongSwitch("interactive")]
    public string? Interactive { get; set; }

    [CommandLongSwitch("privileged")]
    public string? Privileged { get; set; }

    [CommandLongSwitch("tty")]
    public string? Tty { get; set; }

    [CommandLongSwitch("user")]
    public string? User { get; set; }

}