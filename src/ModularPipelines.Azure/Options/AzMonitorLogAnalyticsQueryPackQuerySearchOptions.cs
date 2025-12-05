using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "query-pack", "query", "search")]
public record AzMonitorLogAnalyticsQueryPackQuerySearchOptions : AzOptions
{
    [CliOption("--categories")]
    public string? Categories { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--include-body")]
    public bool? IncludeBody { get; set; }

    [CliOption("--query-pack-name")]
    public string? QueryPackName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-types")]
    public string? ResourceTypes { get; set; }

    [CliOption("--solutions")]
    public string? Solutions { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}