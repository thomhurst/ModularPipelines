using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-server-config", "set")]
public record AzNetworkVpnServerConfigSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-audience")]
    public string? AadAudience { get; set; }

    [CommandSwitch("--aad-issuer")]
    public string? AadIssuer { get; set; }

    [CommandSwitch("--aad-tenant")]
    public string? AadTenant { get; set; }

    [CommandSwitch("--auth-types")]
    public string? AuthTypes { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocols")]
    public string? Protocols { get; set; }

    [CommandSwitch("--radius-client-root-certs")]
    public string? RadiusClientRootCerts { get; set; }

    [CommandSwitch("--radius-server-root-certs")]
    public string? RadiusServerRootCerts { get; set; }

    [CommandSwitch("--radius-servers")]
    public string? RadiusServers { get; set; }

    [CommandSwitch("--vpn-client-revoked-certs")]
    public string? VpnClientRevokedCerts { get; set; }

    [CommandSwitch("--vpn-client-root-certs")]
    public string? VpnClientRootCerts { get; set; }
}

