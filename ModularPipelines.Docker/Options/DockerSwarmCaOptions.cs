using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm ca")]
public record DockerSwarmCaOptions : DockerOptions
{
    [CommandLongSwitch("ca-cert")]
    public string? CaCert { get; set; }

    [CommandLongSwitch("ca-key")]
    public string? CaKey { get; set; }

    [CommandLongSwitch("cert-expiry")]
    public string? CertExpiry { get; set; }

    [CommandLongSwitch("external-ca")]
    public string? ExternalCa { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}