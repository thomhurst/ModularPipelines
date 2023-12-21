using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb", "server", "georestore")]
public record AzMariadbServerGeorestoreOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--source-server")] string SourceServer
) : AzOptions
{
    [CommandSwitch("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CommandSwitch("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sku-name")]
    public string? SkuName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}