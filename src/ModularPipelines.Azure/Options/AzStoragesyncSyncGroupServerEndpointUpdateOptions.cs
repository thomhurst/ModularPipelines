using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagesync", "sync-group", "server-endpoint", "update")]
public record AzStoragesyncSyncGroupServerEndpointUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--storage-sync-service")] string StorageSyncService,
[property: CommandSwitch("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CommandSwitch("--cloud-tiering")]
    public string? CloudTiering { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--offline-data-transfer")]
    public string? OfflineDataTransfer { get; set; }

    [CommandSwitch("--offline-data-transfer-share-name")]
    public string? OfflineDataTransferShareName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--tier-files-older-than-days")]
    public string? TierFilesOlderThanDays { get; set; }

    [CommandSwitch("--volume-free-space-percent")]
    public string? VolumeFreeSpacePercent { get; set; }
}