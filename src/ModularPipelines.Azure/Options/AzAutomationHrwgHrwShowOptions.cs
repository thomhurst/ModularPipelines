using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "hrwg", "hrw", "show")]
public record AzAutomationHrwgHrwShowOptions : AzOptions
{
    [CliOption("--automation-account-name")]
    public int? AutomationAccountName { get; set; }

    [CliOption("--hybrid-runbook-worker-group-name")]
    public string? HybridRunbookWorkerGroupName { get; set; }

    [CliOption("--hybrid-runbook-worker-id")]
    public string? HybridRunbookWorkerId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}