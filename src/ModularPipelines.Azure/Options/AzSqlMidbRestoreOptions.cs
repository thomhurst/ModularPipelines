using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "midb", "restore")]
public record AzSqlMidbRestoreOptions(
[property: CliOption("--dest-name")] string DestName,
[property: CliOption("--time")] string Time
) : AzOptions
{
    [CliOption("--deleted-time")]
    public string? DeletedTime { get; set; }

    [CliOption("--dest-mi")]
    public string? DestMi { get; set; }

    [CliOption("--dest-resource-group")]
    public string? DestResourceGroup { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source-sub")]
    public string? SourceSub { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}