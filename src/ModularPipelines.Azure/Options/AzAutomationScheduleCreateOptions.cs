using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automation", "schedule", "create")]
public record AzAutomationScheduleCreateOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--frequency")] string Frequency,
[property: CliOption("--interval")] int Interval,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--expiry-time")]
    public string? ExpiryTime { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }
}