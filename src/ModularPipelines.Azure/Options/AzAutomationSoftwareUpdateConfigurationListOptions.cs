using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "software-update-configuration", "list")]
public record AzAutomationSoftwareUpdateConfigurationListOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;