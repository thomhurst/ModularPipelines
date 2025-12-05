using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "cloud-endpoint", "create")]
public record AzStoragesyncSyncGroupCloudEndpointCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--storage-sync-service")] string StorageSyncService,
[property: CliOption("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CliOption("--azure-file-share-name")]
    public string? AzureFileShareName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--storage-account-tenant-id")]
    public int? StorageAccountTenantId { get; set; }
}