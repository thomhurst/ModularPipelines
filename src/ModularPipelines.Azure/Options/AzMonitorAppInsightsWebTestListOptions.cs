using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "web-test", "list")]
public record AzMonitorAppInsightsWebTestListOptions : AzOptions
{
    [CommandSwitch("--component-name")]
    public string? ComponentName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}