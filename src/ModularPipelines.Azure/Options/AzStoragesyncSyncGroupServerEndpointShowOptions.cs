using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "server-endpoint", "show")]
public record AzStoragesyncSyncGroupServerEndpointShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--storage-sync-service")] string StorageSyncService,
[property: CliOption("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}