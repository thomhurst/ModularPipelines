using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerOptions() : CommandLineToolOptions("docker")
{
    [CliOption("--config")]
    public virtual string? Config { get; set; }

    [CliOption("--context")]
    public virtual string? Context { get; set; }

    [CliOption("--debug")]
    public virtual string? Debug { get; set; }

    [CliOption("--help")]
    public virtual string? Help { get; set; }

    [CliOption("--host")]
    public virtual string? Host { get; set; }

    [CliOption("--log-level")]
    public virtual string? LogLevel { get; set; }

    [CliOption("--tls")]
    public virtual string? Tls { get; set; }

    [CliOption("--tlscacert")]
    public virtual string? Tlscacert { get; set; }

    [CliOption("--tlscert")]
    public virtual string? Tlscert { get; set; }

    [CliOption("--tlskey")]
    public virtual string? Tlskey { get; set; }

    [CliOption("--tlsverify")]
    public virtual string? Tlsverify { get; set; }
}
