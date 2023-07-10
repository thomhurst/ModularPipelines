using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("docker")]
public record DockerBaseOptions : DockerOptions
{
    [CommandLongSwitch("config")]
    public string? Config { get; set; }

    [CommandLongSwitch("context")]
    public string? Context { get; set; }

    [CommandLongSwitch("debug")]
    public string? Debug { get; set; }

    [CommandLongSwitch("host")]
    public string? Host { get; set; }

    [CommandLongSwitch("log-level")]
    public string? LogLevel { get; set; }

    [CommandLongSwitch("tls")]
    public string? Tls { get; set; }

    [CommandLongSwitch("tlscacert")]
    public string? Tlscacert { get; set; }

    [CommandLongSwitch("tlscert")]
    public string? Tlscert { get; set; }

    [CommandLongSwitch("tlskey")]
    public string? Tlskey { get; set; }

    [CommandLongSwitch("tlsverify")]
    public string? Tlsverify { get; set; }

}