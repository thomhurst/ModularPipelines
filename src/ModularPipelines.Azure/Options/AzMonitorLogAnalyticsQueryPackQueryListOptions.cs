using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "query-pack", "query", "list")]
public record AzMonitorLogAnalyticsQueryPackQueryListOptions(
[property: CommandSwitch("--query-pack-name")] string QueryPackName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--include-body")]
    public bool? IncludeBody { get; set; }
}