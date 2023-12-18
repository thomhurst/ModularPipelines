using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "table", "search-job", "create")]
public record AzMonitorLogAnalyticsWorkspaceTableSearchJobCreateOptions(
[property: CommandSwitch("--end-search-time")] string EndSearchTime,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--search-query")] string SearchQuery,
[property: CommandSwitch("--start-search-time")] string StartSearchTime,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--retention-time")]
    public string? RetentionTime { get; set; }

    [CommandSwitch("--total-retention-time")]
    public string? TotalRetentionTime { get; set; }
}