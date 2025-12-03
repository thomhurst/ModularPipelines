using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "invoice", "section", "create")]
public record AzBillingInvoiceSectionCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--invoice-section-name")] string InvoiceSectionName,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}