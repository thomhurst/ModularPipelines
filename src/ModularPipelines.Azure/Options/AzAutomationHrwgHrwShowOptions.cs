using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "hrwg", "hrw", "show")]
public record AzAutomationHrwgHrwShowOptions : AzOptions
{
    [CommandSwitch("--automation-account-name")]
    public int? AutomationAccountName { get; set; }

    [CommandSwitch("--hybrid-runbook-worker-group-name")]
    public string? HybridRunbookWorkerGroupName { get; set; }

    [CommandSwitch("--hybrid-runbook-worker-id")]
    public string? HybridRunbookWorkerId { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}