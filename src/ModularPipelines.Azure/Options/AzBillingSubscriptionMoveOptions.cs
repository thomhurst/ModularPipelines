using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "subscription", "move")]
public record AzBillingSubscriptionMoveOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--destination-invoice-section-id")] string DestinationInvoiceSectionId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}