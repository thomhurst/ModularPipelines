using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "troubleshooting", "start")]
public record AzNetworkWatcherTroubleshootingStartOptions(
[property: CliOption("--resource")] string Resource,
[property: CliOption("--storage-account")] int StorageAccount,
[property: CliOption("--storage-path")] string StoragePath
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--watcher-rg")]
    public string? WatcherRg { get; set; }
}