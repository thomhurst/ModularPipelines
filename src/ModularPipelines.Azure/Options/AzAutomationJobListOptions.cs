using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "job", "list")]
public record AzAutomationJobListOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;