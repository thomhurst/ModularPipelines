using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "server-endpoint", "list")]
public record AzStoragesyncSyncGroupServerEndpointListOptions(
[property: CliOption("--storage-sync-service")] string StorageSyncService,
[property: CliOption("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}