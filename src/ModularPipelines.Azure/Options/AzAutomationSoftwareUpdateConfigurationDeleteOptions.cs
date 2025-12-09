using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "software-update-configuration", "delete")]
public record AzAutomationSoftwareUpdateConfigurationDeleteOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}