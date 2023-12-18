using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "python3-package", "list")]
public record AzAutomationPython3PackageListOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;