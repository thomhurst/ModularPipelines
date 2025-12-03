using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automation", "runbook", "create")]
public record AzAutomationRunbookCreateOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-activity-trace")]
    public string? LogActivityTrace { get; set; }

    [CliFlag("--log-progress")]
    public bool? LogProgress { get; set; }

    [CliFlag("--log-verbose")]
    public bool? LogVerbose { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}