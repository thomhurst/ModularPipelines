using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "agreement", "list")]
public record AzBillingAgreementListOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}