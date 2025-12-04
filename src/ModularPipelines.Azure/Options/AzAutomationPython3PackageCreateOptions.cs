using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "python3-package", "create")]
public record AzAutomationPython3PackageCreateOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--content-link")] string ContentLink,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}