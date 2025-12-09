using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "ssl-policy", "set")]
public record AzNetworkApplicationGatewaySslPolicySetOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cipher-suites")]
    public string? CipherSuites { get; set; }

    [CliFlag("--disabled-ssl-protocols")]
    public bool? DisabledSslProtocols { get; set; }

    [CliOption("--min-protocol-version")]
    public string? MinProtocolVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--policy-type")]
    public string? PolicyType { get; set; }
}