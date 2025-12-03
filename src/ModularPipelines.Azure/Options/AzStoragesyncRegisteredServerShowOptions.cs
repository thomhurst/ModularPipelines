using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagesync", "registered-server", "show")]
public record AzStoragesyncRegisteredServerShowOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--storage-sync-service")] string StorageSyncService
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}