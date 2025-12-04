using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storagesync", "sync-group", "server-endpoint", "wait")]
public record AzStoragesyncSyncGroupServerEndpointWaitOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-sync-service-name")] string StorageSyncServiceName,
[property: CliOption("--sync-group-name")] string SyncGroupName
) : AzOptions
{
    [CliFlag("--created")]
    public bool? Created { get; set; }

    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliFlag("--exists")]
    public bool? Exists { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}