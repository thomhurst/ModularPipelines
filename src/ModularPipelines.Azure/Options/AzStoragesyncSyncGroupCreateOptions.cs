using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "create")]
public record AzStoragesyncSyncGroupCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--storage-sync-service")] string StorageSyncService
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}