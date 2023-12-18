using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "agreement", "show")]
public record AzBillingAgreementShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}