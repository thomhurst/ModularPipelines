using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "create")]
public record AzCosmosdbCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--analytical-storage-schema-type")]
    public string? AnalyticalStorageSchemaType { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

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

    [CliOption("--databases-to-restore")]
    public string? DatabasesToRestore { get; set; }

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

    [CliFlag("--enable-free-tier")]
    public bool? EnableFreeTier { get; set; }

    [CliFlag("--enable-multiple-write-locations")]
    public bool? EnableMultipleWriteLocations { get; set; }

    [CliFlag("--enable-partition-merge")]
    public bool? EnablePartitionMerge { get; set; }

    [CliFlag("--enable-virtual-network")]
    public bool? EnableVirtualNetwork { get; set; }

    [CliOption("--gremlin-databases-to-restore")]
    public string? GremlinDatabasesToRestore { get; set; }

    [CliOption("--ip-range-filter")]
    public string? IpRangeFilter { get; set; }

    [CliFlag("--is-restore-request")]
    public bool? IsRestoreRequest { get; set; }

    [CliOption("--key-uri")]
    public string? KeyUri { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--locations")]
    public string? Locations { get; set; }

    [CliOption("--max-interval")]
    public string? MaxInterval { get; set; }

    [CliOption("--max-staleness-prefix")]
    public string? MaxStalenessPrefix { get; set; }

    [CliOption("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CliOption("--network-acl-bypass")]
    public string? NetworkAclBypass { get; set; }

    [CliOption("--network-acl-bypass-resource-ids")]
    public string? NetworkAclBypassResourceIds { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--restore-source")]
    public string? RestoreSource { get; set; }

    [CliOption("--restore-timestamp")]
    public string? RestoreTimestamp { get; set; }

    [CliOption("--server-version")]
    public string? ServerVersion { get; set; }

    [CliOption("--tables-to-restore")]
    public string? TablesToRestore { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}