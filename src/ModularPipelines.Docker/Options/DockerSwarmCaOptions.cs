using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm ca")]
[ExcludeFromCodeCoverage]
public record DockerSwarmCaOptions : DockerOptions
{
    [CommandSwitch("--ca-cert")]
    public string? CaCert { get; set; }

    [CommandSwitch("--ca-key")]
    public string? CaKey { get; set; }

    [CommandSwitch("--cert-expiry")]
    public string? CertExpiry { get; set; }

    [CommandSwitch("--external-ca")]
    public string? ExternalCa { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--rotate")]
    public string? Rotate { get; set; }
}
