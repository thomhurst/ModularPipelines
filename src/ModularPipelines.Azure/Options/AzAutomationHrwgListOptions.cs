using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation", "hrwg", "list")]
public record AzAutomationHrwgListOptions(
[property: CliOption("--automation-account-name")] int AutomationAccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}