using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "packet-capture", "show")]
public record AzNetworkWatcherPacketCaptureShowOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name
) : AzOptions;