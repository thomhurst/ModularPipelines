using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "linked-storage", "create")]
public record AzMonitorLogAnalyticsWorkspaceLinkedStorageCreateOptions(
[property: CliOption("--data-source-type")] string DataSourceType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-accounts")] int StorageAccounts,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;