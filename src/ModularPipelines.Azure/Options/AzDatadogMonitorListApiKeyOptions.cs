using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datadog", "monitor", "list-api-key")]
public record AzDatadogMonitorListApiKeyOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;