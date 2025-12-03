using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "flexible-server", "replica", "create")]
public record AzMysqlFlexibleServerReplicaCreateOptions(
[property: CliOption("--replica-name")] string ReplicaName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-server")] string SourceServer
) : AzOptions
{
    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliOption("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CliOption("--iops")]
    public string? Iops { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--public-access")]
    public string? PublicAccess { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}