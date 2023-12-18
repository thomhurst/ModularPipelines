using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "snapshot", "restore")]
public record AzWebappConfigSnapshotRestoreOptions(
[property: CommandSwitch("--time")] string Time
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--restore-content-only")]
    public bool? RestoreContentOnly { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--source-name")]
    public string? SourceName { get; set; }

    [CommandSwitch("--source-resource-group")]
    public string? SourceResourceGroup { get; set; }

    [CommandSwitch("--source-slot")]
    public string? SourceSlot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}