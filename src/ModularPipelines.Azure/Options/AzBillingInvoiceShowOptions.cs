using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "invoice", "show")]
public record AzBillingInvoiceShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--by-subscription")]
    public string? BySubscription { get; set; }
}