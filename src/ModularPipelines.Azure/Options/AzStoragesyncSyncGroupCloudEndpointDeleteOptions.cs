using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagesync", "sync-group", "cloud-endpoint", "delete")]
public record AzStoragesyncSyncGroupCloudEndpointDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--storage-sync-service")] string StorageSyncService,
[property: CommandSwitch("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}