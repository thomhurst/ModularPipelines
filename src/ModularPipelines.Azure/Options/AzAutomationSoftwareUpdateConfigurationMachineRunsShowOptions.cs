using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automation", "software-update-configuration", "machine-runs", "show")]
public record AzAutomationSoftwareUpdateConfigurationMachineRunsShowOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--software-update-configuration-machine-run-id")] string SoftwareUpdateConfigurationMachineRunId
) : AzOptions;