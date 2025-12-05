using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datadog", "monitor", "list-host")]
public record AzDatadogMonitorListHostOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;