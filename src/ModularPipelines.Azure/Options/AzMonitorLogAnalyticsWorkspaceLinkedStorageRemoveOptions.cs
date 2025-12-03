using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "linked-storage", "remove")]
public record AzMonitorLogAnalyticsWorkspaceLinkedStorageRemoveOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-accounts")] int StorageAccounts,
[property: CliOption("--type")] string Type,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;