using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagesync", "sync-group", "cloud-endpoint", "list")]
public record AzStoragesyncSyncGroupCloudEndpointListOptions(
[property: CommandSwitch("--storage-sync-service")] string StorageSyncService,
[property: CommandSwitch("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}