using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerOptions() : CommandLineToolOptions("docker")
{
    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [CommandSwitch("--context")]
    public string? ContextOverride { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--debug")]
    public bool? Debug { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [BooleanCommandSwitch("--tls")]
    public bool? Tls { get; set; }

    [CommandSwitch("--tlscacert")]
    public string? TlsCaCert { get; set; }

    [CommandSwitch("--tlscert")]
    public string? TlsCert { get; set; }

    [CommandSwitch("--tlskey")]
    public string? TlsKey { get; set; }

    [BooleanCommandSwitch("--tlsverify")]
    public bool? TlsVerify { get; set; }
}
