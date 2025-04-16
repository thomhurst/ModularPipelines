using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerOptions() : CommandLineToolOptions("docker")
{
    [CommandSwitch("--config")]
    public virtual string? Config { get; set; }

    [CommandSwitch("--context")]
    public virtual string? Context { get; set; }

    [CommandSwitch("--debug")]
    public virtual string? Debug { get; set; }

    [CommandSwitch("--help")]
    public virtual string? Help { get; set; }

    [CommandSwitch("--host")]
    public virtual string? Host { get; set; }

    [CommandSwitch("--log-level")]
    public virtual string? LogLevel { get; set; }

    [CommandSwitch("--tls")]
    public virtual string? Tls { get; set; }

    [CommandSwitch("--tlscacert")]
    public virtual string? Tlscacert { get; set; }

    [CommandSwitch("--tlscert")]
    public virtual string? Tlscert { get; set; }

    [CommandSwitch("--tlskey")]
    public virtual string? Tlskey { get; set; }

    [CommandSwitch("--tlsverify")]
    public virtual string? Tlsverify { get; set; }
}
