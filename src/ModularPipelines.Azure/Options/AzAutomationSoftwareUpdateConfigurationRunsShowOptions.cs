using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "software-update-configuration", "runs", "show")]
public record AzAutomationSoftwareUpdateConfigurationRunsShowOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--software-update-configuration-run-id")] string SoftwareUpdateConfigurationRunId
) : AzOptions
{
}

