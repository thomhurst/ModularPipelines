using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "hrwg", "hrw", "list")]
public record AzAutomationHrwgHrwListOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--hybrid-runbook-worker-group-name")] string HybridRunbookWorkerGroupName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}