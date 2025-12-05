using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "packet-capture", "stop")]
public record AzNetworkWatcherPacketCaptureStopOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}