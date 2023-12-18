using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "log-replay", "start")]
public record AzSqlMidbLogReplayStartOptions(
[property: CommandSwitch("--ss")] string Ss,
[property: CommandSwitch("--storage-uri")] string StorageUri
) : AzOptions
{
    [BooleanCommandSwitch("--auto-complete")]
    public bool? AutoComplete { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--last-backup-name")]
    public string? LastBackupName { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--si")]
    public string? Si { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}