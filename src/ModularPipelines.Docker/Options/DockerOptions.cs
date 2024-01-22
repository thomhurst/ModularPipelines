using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerOptions() : CommandLineToolOptions("docker")
{
    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--debug")]
    public string? Debug { get; set; }

    [CommandSwitch("--help")]
    public string? Help { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--tls")]
    public string? Tls { get; set; }

    [CommandSwitch("--tlscacert")]
    public string? Tlscacert { get; set; }

    [CommandSwitch("--tlscert")]
    public string? Tlscert { get; set; }

    [CommandSwitch("--tlskey")]
    public string? Tlskey { get; set; }

    [CommandSwitch("--tlsverify")]
    public string? Tlsverify { get; set; }
}
