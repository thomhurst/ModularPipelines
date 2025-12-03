using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "packet-capture", "show")]
public record AzNetworkWatcherPacketCaptureShowOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name
) : AzOptions;