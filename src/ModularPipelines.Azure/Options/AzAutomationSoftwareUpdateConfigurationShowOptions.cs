using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "software-update-configuration", "show")]
public record AzAutomationSoftwareUpdateConfigurationShowOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;