using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "list")]
public record AzNetworkWatcherConnectionMonitorListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}