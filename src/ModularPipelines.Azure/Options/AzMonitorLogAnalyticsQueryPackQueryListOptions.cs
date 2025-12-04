using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "query-pack", "query", "list")]
public record AzMonitorLogAnalyticsQueryPackQueryListOptions(
[property: CliOption("--query-pack-name")] string QueryPackName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--include-body")]
    public bool? IncludeBody { get; set; }
}