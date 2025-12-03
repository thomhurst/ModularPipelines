using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "customer", "show")]
public record AzBillingCustomerShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--customer-name")] string CustomerName
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}