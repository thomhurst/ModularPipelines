using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "subscription", "validate-move")]
public record AzBillingSubscriptionValidateMoveOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--destination-invoice-section-id")] string DestinationInvoiceSectionId
) : AzOptions;