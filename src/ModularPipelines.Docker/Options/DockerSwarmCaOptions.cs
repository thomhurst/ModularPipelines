using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("swarm", "ca")]
[ExcludeFromCodeCoverage]
public record DockerSwarmCaOptions : DockerOptions
{
    [CliOption("--ca-cert")]
    public virtual string? CaCert { get; set; }

    [CliOption("--ca-key")]
    public virtual string? CaKey { get; set; }

    [CliOption("--cert-expiry")]
    public virtual string? CertExpiry { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--external-ca")]
    public virtual string? ExternalCa { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--rotate")]
    public virtual string? Rotate { get; set; }
}
