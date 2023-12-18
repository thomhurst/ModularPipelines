using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-profile", "update")]
public record AzNetworkApplicationGatewaySslProfileUpdateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--cipher-suites")]
    public string? CipherSuites { get; set; }

    [BooleanCommandSwitch("--client-auth-config")]
    public bool? ClientAuthConfig { get; set; }

    [BooleanCommandSwitch("--disabled-protocols")]
    public bool? DisabledProtocols { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--min-protocol-version")]
    public string? MinProtocolVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--policy-type")]
    public string? PolicyType { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--trusted-client-cert")]
    public string? TrustedClientCert { get; set; }
}