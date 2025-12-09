using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "server", "georestore")]
public record AzPostgresServerGeorestoreOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--source-server")] string SourceServer
) : AzOptions
{
    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliOption("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}