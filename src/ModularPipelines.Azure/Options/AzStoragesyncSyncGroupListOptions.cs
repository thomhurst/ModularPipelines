using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "list")]
public record AzStoragesyncSyncGroupListOptions(
[property: CliOption("--storage-sync-service")] string StorageSyncService
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}