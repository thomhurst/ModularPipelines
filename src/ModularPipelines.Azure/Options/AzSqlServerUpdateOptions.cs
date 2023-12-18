using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "update")]
public record AzSqlServerUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [BooleanCommandSwitch("--assign_identity")]
    public bool? AssignIdentity { get; set; }

    [BooleanCommandSwitch("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [CommandSwitch("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pid")]
    public string? Pid { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--restrict-outbound-network-access")]
    public bool? RestrictOutboundNetworkAccess { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--user-assigned-identity-id")]
    public string? UserAssignedIdentityId { get; set; }
}