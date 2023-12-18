using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagesync", "sync-group", "server-endpoint", "create")]
public record AzStoragesyncSyncGroupServerEndpointCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registered-server-id")] string RegisteredServerId,
[property: CommandSwitch("--server-local-path")] string ServerLocalPath,
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