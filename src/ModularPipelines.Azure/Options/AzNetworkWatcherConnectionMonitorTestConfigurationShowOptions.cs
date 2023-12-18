using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "connection-monitor", "test-configuration", "show")]
public record AzNetworkWatcherConnectionMonitorTestConfigurationShowOptions(
[property: CommandSwitch("--connection-monitor")] string ConnectionMonitor,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name
) : AzOptions;