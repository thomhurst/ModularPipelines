using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "product", "validate-move")]
public record AzBillingProductValidateMoveOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--destination-invoice-section-id")]
    public string? DestinationInvoiceSectionId { get; set; }
}