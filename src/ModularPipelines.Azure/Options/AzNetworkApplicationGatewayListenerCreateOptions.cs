using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "listener", "create")]
public record AzNetworkApplicationGatewayListenerCreateOptions(
[property: CliOption("--frontend-port")] string FrontendPort,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CliOption("--host-names")]
    public string? HostNames { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--ssl-cert")]
    public string? SslCert { get; set; }

    [CliOption("--ssl-profile-id")]
    public string? SslProfileId { get; set; }
}