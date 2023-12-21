using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "update")]
public record AzCosmosdbUpdateOptions : AzOptions
{
    [CommandSwitch("--analytical-storage-schema-type")]
    public string? AnalyticalStorageSchemaType { get; set; }

    [CommandSwitch("--backup-interval")]
    public string? BackupInterval { get; set; }

    [CommandSwitch("--backup-policy-type")]
    public string? BackupPolicyType { get; set; }

    [CommandSwitch("--backup-redundancy")]
    public string? BackupRedundancy { get; set; }

    [CommandSwitch("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CommandSwitch("--capabilities")]
    public string? Capabilities { get; set; }

    [CommandSwitch("--continuous-tier")]
    public string? ContinuousTier { get; set; }

    [CommandSwitch("--default-consistency-level")]
    public string? DefaultConsistencyLevel { get; set; }

    [CommandSwitch("--default-identity")]
    public string? DefaultIdentity { get; set; }

    [BooleanCommandSwitch("--disable-key-based-metadata-write-access")]
    public bool? DisableKeyBasedMetadataWriteAccess { get; set; }

    [BooleanCommandSwitch("--enable-analytical-storage")]
    public bool? EnableAnalyticalStorage { get; set; }

    [BooleanCommandSwitch("--enable-automatic-failover")]
    public bool? EnableAutomaticFailover { get; set; }

    [BooleanCommandSwitch("--enable-burst-capacity")]
    public bool? EnableBurstCapacity { get; set; }

    [BooleanCommandSwitch("--enable-multiple-write-locations")]
    public bool? EnableMultipleWriteLocations { get; set; }

    [BooleanCommandSwitch("--enable-partition-merge")]
    public bool? EnablePartitionMerge { get; set; }

    [BooleanCommandSwitch("--enable-virtual-network")]
    public bool? EnableVirtualNetwork { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-range-filter")]
    public string? IpRangeFilter { get; set; }

    [CommandSwitch("--key-uri")]
    public string? KeyUri { get; set; }

    [CommandSwitch("--locations")]
    public string? Locations { get; set; }

    [CommandSwitch("--max-interval")]
    public string? MaxInterval { get; set; }

    [CommandSwitch("--max-staleness-prefix")]
    public string? MaxStalenessPrefix { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network-acl-bypass")]
    public string? NetworkAclBypass { get; set; }

    [CommandSwitch("--network-acl-bypass-resource-ids")]
    public string? NetworkAclBypassResourceIds { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server-version")]
    public string? ServerVersion { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}