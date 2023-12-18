using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "table", "restore", "create")]
public record AzMonitorLogAnalyticsWorkspaceTableRestoreCreateOptions(
[property: CommandSwitch("--end-restore-time")] string EndRestoreTime,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--restore-source-table")] string RestoreSourceTable,
[property: CommandSwitch("--start-restore-time")] string StartRestoreTime,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}