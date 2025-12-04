using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "component", "continues-export", "list")]
public record AzMonitorAppInsightsComponentContinuesExportListOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;