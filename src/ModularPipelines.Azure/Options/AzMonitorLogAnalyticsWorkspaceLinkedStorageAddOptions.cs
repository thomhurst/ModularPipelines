using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "linked-storage", "add")]
public record AzMonitorLogAnalyticsWorkspaceLinkedStorageAddOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-accounts")] int StorageAccounts,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;