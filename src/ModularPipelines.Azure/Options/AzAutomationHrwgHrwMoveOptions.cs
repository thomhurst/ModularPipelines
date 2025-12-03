using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automation", "hrwg", "hrw", "move")]
public record AzAutomationHrwgHrwMoveOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--hybrid-runbook-worker-group-name")] string HybridRunbookWorkerGroupName,
[property: CliOption("--hybrid-runbook-worker-id")] string HybridRunbookWorkerId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;