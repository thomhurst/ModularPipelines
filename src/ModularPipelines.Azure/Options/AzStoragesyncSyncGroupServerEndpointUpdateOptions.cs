using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "server-endpoint", "update")]
public record AzStoragesyncSyncGroupServerEndpointUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--storage-sync-service")] string StorageSyncService,
[property: CliOption("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CliOption("--cloud-tiering")]
    public string? CloudTiering { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--offline-data-transfer")]
    public string? OfflineDataTransfer { get; set; }

    [CliOption("--offline-data-transfer-share-name")]
    public string? OfflineDataTransferShareName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tier-files-older-than-days")]
    public string? TierFilesOlderThanDays { get; set; }

    [CliOption("--volume-free-space-percent")]
    public string? VolumeFreeSpacePercent { get; set; }
}