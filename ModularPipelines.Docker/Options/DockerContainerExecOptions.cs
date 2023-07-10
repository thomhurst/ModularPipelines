using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container exec")]
public record DockerContainerExecOptions : DockerOptions
{
    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("detach-keys")]
    public string? DetachKeys { get; set; }

    [CommandLongSwitch("env")]
    public string? Env { get; set; }

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

    [CommandLongSwitch("workdir")]
    public string? Workdir { get; set; }

}