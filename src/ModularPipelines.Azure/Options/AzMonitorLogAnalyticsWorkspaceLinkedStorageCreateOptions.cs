using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "linked-storage", "create")]
public record AzMonitorLogAnalyticsWorkspaceLinkedStorageCreateOptions(
[property: CommandSwitch("--data-source-type")] string DataSourceType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-accounts")] int StorageAccounts,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}