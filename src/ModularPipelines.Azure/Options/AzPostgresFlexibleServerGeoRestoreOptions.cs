using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "geo-restore")]
public record AzPostgresFlexibleServerGeoRestoreOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--source-server")] string SourceServer
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--backup-identity")]
    public string? BackupIdentity { get; set; }

    [CliOption("--backup-key")]
    public string? BackupKey { get; set; }

    [CliOption("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}