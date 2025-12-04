using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "update")]
public record AzCosmosdbUpdateOptions : AzOptions
{
    [CliOption("--analytical-storage-schema-type")]
    public string? AnalyticalStorageSchemaType { get; set; }

    [CliOption("--backup-interval")]
    public string? BackupInterval { get; set; }

    [CliOption("--backup-policy-type")]
    public string? BackupPolicyType { get; set; }

    [CliOption("--backup-redundancy")]
    public string? BackupRedundancy { get; set; }

    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliOption("--capabilities")]
    public string? Capabilities { get; set; }

    [CliOption("--continuous-tier")]
    public string? ContinuousTier { get; set; }

    [CliOption("--default-consistency-level")]
    public string? DefaultConsistencyLevel { get; set; }

    [CliOption("--default-identity")]
    public string? DefaultIdentity { get; set; }

    [CliFlag("--disable-key-based-metadata-write-access")]
    public bool? DisableKeyBasedMetadataWriteAccess { get; set; }

    [CliFlag("--enable-analytical-storage")]
    public bool? EnableAnalyticalStorage { get; set; }

    [CliFlag("--enable-automatic-failover")]
    public bool? EnableAutomaticFailover { get; set; }

    [CliFlag("--enable-burst-capacity")]
    public bool? EnableBurstCapacity { get; set; }

    [CliFlag("--enable-multiple-write-locations")]
    public bool? EnableMultipleWriteLocations { get; set; }

    [CliFlag("--enable-partition-merge")]
    public bool? EnablePartitionMerge { get; set; }

    [CliFlag("--enable-virtual-network")]
    public bool? EnableVirtualNetwork { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ip-range-filter")]
    public string? IpRangeFilter { get; set; }

    [CliOption("--key-uri")]
    public string? KeyUri { get; set; }

    [CliOption("--locations")]
    public string? Locations { get; set; }

    [CliOption("--max-interval")]
    public string? MaxInterval { get; set; }

    [CliOption("--max-staleness-prefix")]
    public string? MaxStalenessPrefix { get; set; }

    [CliOption("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network-acl-bypass")]
    public string? NetworkAclBypass { get; set; }

    [CliOption("--network-acl-bypass-resource-ids")]
    public string? NetworkAclBypassResourceIds { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server-version")]
    public string? ServerVersion { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}