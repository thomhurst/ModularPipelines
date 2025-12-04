using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "component", "connect-webapp")]
public record AzMonitorAppInsightsComponentConnectWebappOptions(
[property: CliOption("--web-app")] string WebApp
) : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliFlag("--enable-debugger")]
    public bool? EnableDebugger { get; set; }

    [CliFlag("--enable-profiler")]
    public bool? EnableProfiler { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}