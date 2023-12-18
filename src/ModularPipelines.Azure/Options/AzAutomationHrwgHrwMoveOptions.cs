using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "hrwg", "hrw", "move")]
public record AzAutomationHrwgHrwMoveOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--hybrid-runbook-worker-group-name")] string HybridRunbookWorkerGroupName,
[property: CommandSwitch("--hybrid-runbook-worker-id")] string HybridRunbookWorkerId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;