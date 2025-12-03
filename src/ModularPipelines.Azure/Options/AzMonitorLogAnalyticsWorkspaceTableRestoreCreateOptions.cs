using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "table", "restore", "create")]
public record AzMonitorLogAnalyticsWorkspaceTableRestoreCreateOptions(
[property: CliOption("--end-restore-time")] string EndRestoreTime,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--restore-source-table")] string RestoreSourceTable,
[property: CliOption("--start-restore-time")] string StartRestoreTime,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}