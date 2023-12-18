using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "create")]
public record AzSqlServerCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-user")]
    public string? AdminUser { get; set; }

    [BooleanCommandSwitch("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [BooleanCommandSwitch("--enable-ad-only-auth")]
    public bool? EnableAdOnlyAuth { get; set; }

    [BooleanCommandSwitch("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [CommandSwitch("--external-admin-name")]
    public string? ExternalAdminName { get; set; }

    [CommandSwitch("--external-admin-principal-type")]
    public string? ExternalAdminPrincipalType { get; set; }

    [CommandSwitch("--external-admin-sid")]
    public string? ExternalAdminSid { get; set; }

    [CommandSwitch("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pid")]
    public string? Pid { get; set; }

    [BooleanCommandSwitch("--restrict-outbound-network-access")]
    public bool? RestrictOutboundNetworkAccess { get; set; }

    [CommandSwitch("--user-assigned-identity-id")]
    public string? UserAssignedIdentityId { get; set; }
}