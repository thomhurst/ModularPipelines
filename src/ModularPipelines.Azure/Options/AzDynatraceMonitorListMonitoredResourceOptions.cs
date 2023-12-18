using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynatrace", "monitor", "list-monitored-resource")]
public record AzDynatraceMonitorListMonitoredResourceOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;