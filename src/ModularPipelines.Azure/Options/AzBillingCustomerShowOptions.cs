using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

