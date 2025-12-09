using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "workspace", "linked-storage", "add")]
public record AzMonitorLogAnalyticsWorkspaceLinkedStorageAddOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-accounts")] int StorageAccounts,
[property: CliOption("--type")] string Type,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;