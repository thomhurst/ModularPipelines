using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "midb", "log-replay", "start")]
public record AzSqlMidbLogReplayStartOptions(
[property: CliOption("--ss")] string Ss,
[property: CliOption("--storage-uri")] string StorageUri
) : AzOptions
{
    [CliFlag("--auto-complete")]
    public bool? AutoComplete { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--last-backup-name")]
    public string? LastBackupName { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--si")]
    public string? Si { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}