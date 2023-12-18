using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "software-update-configuration", "runs", "list")]
public record AzAutomationSoftwareUpdateConfigurationRunsListOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;