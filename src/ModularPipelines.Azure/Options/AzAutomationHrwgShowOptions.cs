using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "hrwg", "show")]
public record AzAutomationHrwgShowOptions : AzOptions
{
    [CommandSwitch("--automation-account-name")]
    public int? AutomationAccountName { get; set; }

    [CommandSwitch("--hybrid-runbook-worker-group-name")]
    public string? HybridRunbookWorkerGroupName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}