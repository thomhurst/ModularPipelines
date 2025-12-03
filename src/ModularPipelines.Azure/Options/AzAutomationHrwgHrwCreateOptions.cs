using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automation", "hrwg", "hrw", "create")]
public record AzAutomationHrwgHrwCreateOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--hybrid-runbook-worker-group-name")] string HybridRunbookWorkerGroupName,
[property: CliOption("--hybrid-runbook-worker-id")] string HybridRunbookWorkerId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--vm-resource-id")]
    public string? VmResourceId { get; set; }
}