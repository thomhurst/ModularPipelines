using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "linked-storage", "delete")]
public record AzMonitorLogAnalyticsWorkspaceLinkedStorageDeleteOptions : AzOptions
{
    [CliOption("--data-source-type")]
    public string? DataSourceType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}