using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "ca")]
[ExcludeFromCodeCoverage]
public record DockerSwarmCaOptions : DockerOptions
{
    [CommandSwitch("--ca-cert")]
    public virtual string? CaCert { get; set; }

    [CommandSwitch("--ca-key")]
    public virtual string? CaKey { get; set; }

    [CommandSwitch("--cert-expiry")]
    public virtual string? CertExpiry { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--external-ca")]
    public virtual string? ExternalCa { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandSwitch("--rotate")]
    public virtual string? Rotate { get; set; }
}
