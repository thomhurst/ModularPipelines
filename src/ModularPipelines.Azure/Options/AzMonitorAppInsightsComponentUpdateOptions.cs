using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component", "update")]
public record AzMonitorAppInsightsComponentUpdateOptions : AzOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ingestion-access")]
    public string? IngestionAccess { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--query-access")]
    public string? QueryAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-time")]
    public string? RetentionTime { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}