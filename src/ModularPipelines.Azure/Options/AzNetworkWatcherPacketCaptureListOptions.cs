using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher", "packet-capture", "list")]
public record AzNetworkWatcherPacketCaptureListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;