using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "app-insights", "events", "show")]
public record AzMonitorAppInsightsEventsShowOptions(
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--event")]
    public string? Event { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--offset")]
    public string? Offset { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}