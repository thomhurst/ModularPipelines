using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automation", "runbook", "update")]
public record AzAutomationRunbookUpdateOptions : AzOptions
{
    [CliOption("--automation-account-name")]
    public int? AutomationAccountName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-activity-trace")]
    public string? LogActivityTrace { get; set; }

    [CliFlag("--log-progress")]
    public bool? LogProgress { get; set; }

    [CliFlag("--log-verbose")]
    public bool? LogVerbose { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}