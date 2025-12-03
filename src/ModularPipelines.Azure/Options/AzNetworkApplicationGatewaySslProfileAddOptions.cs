using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "ssl-profile", "add")]
public record AzNetworkApplicationGatewaySslProfileAddOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cipher-suites")]
    public string? CipherSuites { get; set; }

    [CliFlag("--client-auth-config")]
    public bool? ClientAuthConfig { get; set; }

    [CliFlag("--disabled-protocols")]
    public bool? DisabledProtocols { get; set; }

    [CliOption("--min-protocol-version")]
    public string? MinProtocolVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--policy-type")]
    public string? PolicyType { get; set; }

    [CliOption("--trusted-client-cert")]
    public string? TrustedClientCert { get; set; }
}