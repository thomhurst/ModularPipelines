using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "create")]
public record AzCosmosdbCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytical-storage-schema-type")]
    public string? AnalyticalStorageSchemaType { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

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

    [CommandSwitch("--databases-to-restore")]
    public string? DatabasesToRestore { get; set; }

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

    [BooleanCommandSwitch("--enable-free-tier")]
    public bool? EnableFreeTier { get; set; }

    [BooleanCommandSwitch("--enable-multiple-write-locations")]
    public bool? EnableMultipleWriteLocations { get; set; }

    [BooleanCommandSwitch("--enable-partition-merge")]
    public bool? EnablePartitionMerge { get; set; }

    [BooleanCommandSwitch("--enable-virtual-network")]
    public bool? EnableVirtualNetwork { get; set; }

    [CommandSwitch("--gremlin-databases-to-restore")]
    public string? GremlinDatabasesToRestore { get; set; }

    [CommandSwitch("--ip-range-filter")]
    public string? IpRangeFilter { get; set; }

    [BooleanCommandSwitch("--is-restore-request")]
    public bool? IsRestoreRequest { get; set; }

    [CommandSwitch("--key-uri")]
    public string? KeyUri { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--locations")]
    public string? Locations { get; set; }

    [CommandSwitch("--max-interval")]
    public string? MaxInterval { get; set; }

    [CommandSwitch("--max-staleness-prefix")]
    public string? MaxStalenessPrefix { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CommandSwitch("--network-acl-bypass")]
    public string? NetworkAclBypass { get; set; }

    [CommandSwitch("--network-acl-bypass-resource-ids")]
    public string? NetworkAclBypassResourceIds { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--restore-source")]
    public string? RestoreSource { get; set; }

    [CommandSwitch("--restore-timestamp")]
    public string? RestoreTimestamp { get; set; }

    [CommandSwitch("--server-version")]
    public string? ServerVersion { get; set; }

    [CommandSwitch("--tables-to-restore")]
    public string? TablesToRestore { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-network-rules")]
    public string? VirtualNetworkRules { get; set; }
}

