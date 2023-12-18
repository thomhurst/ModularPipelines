using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "geo-backup", "list")]
public record AzSqlDbGeoBackupListOptions(
[property: CommandSwitch("--dest-database")] string DestDatabase,
[property: CommandSwitch("--dest-server")] string DestServer,
[property: CommandSwitch("--geo-backup-id")] string GeoBackupId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

