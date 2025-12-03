using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "snapshot", "restore")]
public record AzWebappConfigSnapshotRestoreOptions(
[property: CliOption("--time")] string Time
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--restore-content-only")]
    public bool? RestoreContentOnly { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--source-name")]
    public string? SourceName { get; set; }

    [CliOption("--source-resource-group")]
    public string? SourceResourceGroup { get; set; }

    [CliOption("--source-slot")]
    public string? SourceSlot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}