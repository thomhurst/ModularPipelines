using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "cluster", "update", "(kusto", "extension)")]
public record AzKustoClusterUpdateKustoExtensionOptions : AzOptions
{
    [CliOption("--accepted-audiences")]
    public string? AcceptedAudiences { get; set; }

    [CliFlag("--allowed-fqdn-list")]
    public bool? AllowedFqdnList { get; set; }

    [CliFlag("--allowed-ip-range-list")]
    public bool? AllowedIpRangeList { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliFlag("--enable-auto-stop")]
    public bool? EnableAutoStop { get; set; }

    [CliFlag("--enable-disk-encryption")]
    public bool? EnableDiskEncryption { get; set; }

    [CliFlag("--enable-double-encryption")]
    public bool? EnableDoubleEncryption { get; set; }

    [CliFlag("--enable-purge")]
    public bool? EnablePurge { get; set; }

    [CliFlag("--enable-streaming-ingest")]
    public bool? EnableStreamingIngest { get; set; }

    [CliOption("--engine-type")]
    public string? EngineType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--key-vault-properties")]
    public string? KeyVaultProperties { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--optimized-autoscale")]
    public string? OptimizedAutoscale { get; set; }

    [CliOption("--outbound-net-access")]
    public string? OutboundNetAccess { get; set; }

    [CliOption("--public-ip-type")]
    public string? PublicIpType { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--trusted-external-tenants")]
    public string? TrustedExternalTenants { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }

    [CliOption("--vcluster-graduation")]
    public string? VclusterGraduation { get; set; }

    [CliOption("--virtual-network-configuration")]
    public string? VirtualNetworkConfiguration { get; set; }
}