using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "customer", "show")]
public record AzBillingCustomerShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--customer-name")] string CustomerName
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}