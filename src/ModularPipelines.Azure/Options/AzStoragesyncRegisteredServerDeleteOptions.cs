using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "registered-server", "delete")]
public record AzStoragesyncRegisteredServerDeleteOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--storage-sync-service")] string StorageSyncService
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}