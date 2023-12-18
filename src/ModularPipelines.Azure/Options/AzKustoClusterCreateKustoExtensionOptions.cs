using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster", "create", "(kusto", "extension)")]
public record AzKustoClusterCreateKustoExtensionOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--accepted-audiences")]
    public string? AcceptedAudiences { get; set; }

    [BooleanCommandSwitch("--allowed-fqdn-list")]
    public bool? AllowedFqdnList { get; set; }

    [BooleanCommandSwitch("--allowed-ip-range-list")]
    public bool? AllowedIpRangeList { get; set; }

    [BooleanCommandSwitch("--enable-auto-stop")]
    public bool? EnableAutoStop { get; set; }

    [BooleanCommandSwitch("--enable-disk-encryption")]
    public bool? EnableDiskEncryption { get; set; }

    [BooleanCommandSwitch("--enable-double-encryption")]
    public bool? EnableDoubleEncryption { get; set; }

    [BooleanCommandSwitch("--enable-purge")]
    public bool? EnablePurge { get; set; }

    [BooleanCommandSwitch("--enable-streaming-ingest")]
    public bool? EnableStreamingIngest { get; set; }

    [CommandSwitch("--engine-type")]
    public string? EngineType { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--key-vault-properties")]
    public string? KeyVaultProperties { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--optimized-autoscale")]
    public string? OptimizedAutoscale { get; set; }

    [CommandSwitch("--outbound-net-access")]
    public string? OutboundNetAccess { get; set; }

    [CommandSwitch("--public-ip-type")]
    public string? PublicIpType { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--trusted-external-tenants")]
    public string? TrustedExternalTenants { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--user-assigned")]
    public string? UserAssigned { get; set; }

    [CommandSwitch("--vcluster-graduation")]
    public string? VclusterGraduation { get; set; }

    [CommandSwitch("--virtual-network-configuration")]
    public string? VirtualNetworkConfiguration { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}