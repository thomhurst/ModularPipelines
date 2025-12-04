using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "create")]
public record AzSqlServerCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-user")]
    public string? AdminUser { get; set; }

    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliFlag("--enable-ad-only-auth")]
    public bool? EnableAdOnlyAuth { get; set; }

    [CliFlag("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [CliOption("--external-admin-name")]
    public string? ExternalAdminName { get; set; }

    [CliOption("--external-admin-principal-type")]
    public string? ExternalAdminPrincipalType { get; set; }

    [CliOption("--external-admin-sid")]
    public string? ExternalAdminSid { get; set; }

    [CliOption("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pid")]
    public string? Pid { get; set; }

    [CliFlag("--restrict-outbound-network-access")]
    public bool? RestrictOutboundNetworkAccess { get; set; }

    [CliOption("--user-assigned-identity-id")]
    public string? UserAssignedIdentityId { get; set; }
}