using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "workspace", "table", "search-job", "create")]
public record AzMonitorLogAnalyticsWorkspaceTableSearchJobCreateOptions(
[property: CliOption("--end-search-time")] string EndSearchTime,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--search-query")] string SearchQuery,
[property: CliOption("--start-search-time")] string StartSearchTime,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliOption("--total-retention-time")]
    public string? TotalRetentionTime { get; set; }
}