using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component", "connect-webapp")]
public record AzMonitorAppInsightsComponentConnectWebappOptions(
[property: CommandSwitch("--web-app")] string WebApp
) : AzOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [BooleanCommandSwitch("--enable-debugger")]
    public bool? EnableDebugger { get; set; }

    [BooleanCommandSwitch("--enable-profiler")]
    public bool? EnableProfiler { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}