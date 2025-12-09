using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-server-config", "set")]
public record AzNetworkVpnServerConfigSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-audience")]
    public string? AadAudience { get; set; }

    [CliOption("--aad-issuer")]
    public string? AadIssuer { get; set; }

    [CliOption("--aad-tenant")]
    public string? AadTenant { get; set; }

    [CliOption("--auth-types")]
    public string? AuthTypes { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocols")]
    public string? Protocols { get; set; }

    [CliOption("--radius-client-root-certs")]
    public string? RadiusClientRootCerts { get; set; }

    [CliOption("--radius-server-root-certs")]
    public string? RadiusServerRootCerts { get; set; }

    [CliOption("--radius-servers")]
    public string? RadiusServers { get; set; }

    [CliOption("--vpn-client-revoked-certs")]
    public string? VpnClientRevokedCerts { get; set; }

    [CliOption("--vpn-client-root-certs")]
    public string? VpnClientRootCerts { get; set; }
}