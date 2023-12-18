using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "packet-capture", "list")]
public record AzNetworkWatcherPacketCaptureListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;