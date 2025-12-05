using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "software-update-configuration", "runs", "show")]
public record AzAutomationSoftwareUpdateConfigurationRunsShowOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--software-update-configuration-run-id")] string SoftwareUpdateConfigurationRunId
) : AzOptions;