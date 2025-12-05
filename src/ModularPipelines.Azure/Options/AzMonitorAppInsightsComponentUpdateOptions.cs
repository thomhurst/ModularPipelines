using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "component", "update")]
public record AzMonitorAppInsightsComponentUpdateOptions : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ingestion-access")]
    public string? IngestionAccess { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--query-access")]
    public string? QueryAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}