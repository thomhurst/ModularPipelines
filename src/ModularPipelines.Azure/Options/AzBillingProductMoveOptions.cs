using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "product", "move")]
public record AzBillingProductMoveOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--destination-invoice-section-id")]
    public string? DestinationInvoiceSectionId { get; set; }
}