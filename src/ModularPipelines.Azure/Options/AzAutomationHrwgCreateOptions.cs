using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "hrwg", "create")]
public record AzAutomationHrwgCreateOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--hybrid-runbook-worker-group-name")] string HybridRunbookWorkerGroupName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--credential")]
    public string? Credential { get; set; }
}