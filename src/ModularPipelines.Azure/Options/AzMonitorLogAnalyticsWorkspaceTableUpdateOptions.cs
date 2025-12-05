using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "workspace", "table", "update")]
public record AzMonitorLogAnalyticsWorkspaceTableUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--columns")]
    public string? Columns { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliOption("--total-retention-time")]
    public string? TotalRetentionTime { get; set; }
}