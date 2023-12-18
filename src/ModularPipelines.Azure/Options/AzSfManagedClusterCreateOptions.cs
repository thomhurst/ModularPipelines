using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-cluster", "create")]
public record AzSfManagedClusterCreateOptions(
[property: CommandSwitch("--admin-password")] string AdminPassword,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-user-name")]
    public string? AdminUserName { get; set; }

    [CommandSwitch("--cert-common-name")]
    public string? CertCommonName { get; set; }

    [BooleanCommandSwitch("--cert-is-admin")]
    public bool? CertIsAdmin { get; set; }

    [CommandSwitch("--cert-issuer-thumbprint")]
    public string? CertIssuerThumbprint { get; set; }

    [CommandSwitch("--cert-thumbprint")]
    public string? CertThumbprint { get; set; }

    [CommandSwitch("--client-connection-port")]
    public string? ClientConnectionPort { get; set; }

    [CommandSwitch("--cluster-code-version")]
    public string? ClusterCodeVersion { get; set; }

    [CommandSwitch("--cluster-upgrade-cadence")]
    public string? ClusterUpgradeCadence { get; set; }

    [CommandSwitch("--cluster-upgrade-mode")]
    public string? ClusterUpgradeMode { get; set; }

    [CommandSwitch("--dns-name")]
    public string? DnsName { get; set; }

    [CommandSwitch("--gateway-connection-port")]
    public string? GatewayConnectionPort { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}