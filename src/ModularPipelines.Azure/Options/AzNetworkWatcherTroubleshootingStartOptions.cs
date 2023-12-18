using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "troubleshooting", "start")]
public record AzNetworkWatcherTroubleshootingStartOptions(
[property: CommandSwitch("--resource")] string Resource,
[property: CommandSwitch("--storage-account")] int StorageAccount,
[property: CommandSwitch("--storage-path")] string StoragePath
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--watcher-rg")]
    public string? WatcherRg { get; set; }
}