using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-cluster", "create")]
public record AzSfManagedClusterCreateOptions(
[property: CliOption("--admin-password")] string AdminPassword,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-user-name")]
    public string? AdminUserName { get; set; }

    [CliOption("--cert-common-name")]
    public string? CertCommonName { get; set; }

    [CliFlag("--cert-is-admin")]
    public bool? CertIsAdmin { get; set; }

    [CliOption("--cert-issuer-thumbprint")]
    public string? CertIssuerThumbprint { get; set; }

    [CliOption("--cert-thumbprint")]
    public string? CertThumbprint { get; set; }

    [CliOption("--client-connection-port")]
    public string? ClientConnectionPort { get; set; }

    [CliOption("--cluster-code-version")]
    public string? ClusterCodeVersion { get; set; }

    [CliOption("--cluster-upgrade-cadence")]
    public string? ClusterUpgradeCadence { get; set; }

    [CliOption("--cluster-upgrade-mode")]
    public string? ClusterUpgradeMode { get; set; }

    [CliOption("--dns-name")]
    public string? DnsName { get; set; }

    [CliOption("--gateway-connection-port")]
    public string? GatewayConnectionPort { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}