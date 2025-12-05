using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "software-update-configuration", "runs", "list")]
public record AzAutomationSoftwareUpdateConfigurationRunsListOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;