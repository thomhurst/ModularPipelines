using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "app-insights", "web-test", "list")]
public record AzMonitorAppInsightsWebTestListOptions : AzOptions
{
    [CliOption("--component-name")]
    public string? ComponentName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}