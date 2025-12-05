using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "schedule", "update")]
public record AzAutomationScheduleUpdateOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--is-enabled")]
    public bool? IsEnabled { get; set; }
}