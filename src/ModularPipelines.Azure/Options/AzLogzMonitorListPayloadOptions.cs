using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logz", "monitor", "list-payload")]
public record AzLogzMonitorListPayloadOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;