using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component", "continues-export", "list")]
public record AzMonitorAppInsightsComponentContinuesExportListOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;