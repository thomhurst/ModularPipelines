using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "output", "add")]
public record AzNetworkWatcherConnectionMonitorOutputAddOptions(
[property: CommandSwitch("--connection-monitor")] string ConnectionMonitor,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--output-type")] string OutputType
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--output-index")]
    public string? OutputIndex { get; set; }

    [CommandSwitch("--workspace-id")]
    public string? WorkspaceId { get; set; }
}