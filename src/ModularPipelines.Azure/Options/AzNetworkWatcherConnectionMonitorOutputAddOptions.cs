using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "connection-monitor", "output", "add")]
public record AzNetworkWatcherConnectionMonitorOutputAddOptions(
[property: CliOption("--connection-monitor")] string ConnectionMonitor,
[property: CliOption("--location")] string Location,
[property: CliOption("--output-type")] string OutputType
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--output-index")]
    public string? OutputIndex { get; set; }

    [CliOption("--workspace-id")]
    public string? WorkspaceId { get; set; }
}