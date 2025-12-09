using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "cloud-endpoint", "list")]
public record AzStoragesyncSyncGroupCloudEndpointListOptions(
[property: CliOption("--storage-sync-service")] string StorageSyncService,
[property: CliOption("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}