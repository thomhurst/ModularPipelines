using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "python3-package", "create")]
public record AzAutomationPython3PackageCreateOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--content-link")] string ContentLink,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}