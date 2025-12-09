using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "job", "stop")]
public record AzAutomationJobStopOptions : AzOptions
{
    [CliOption("--automation-account-name")]
    public int? AutomationAccountName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}