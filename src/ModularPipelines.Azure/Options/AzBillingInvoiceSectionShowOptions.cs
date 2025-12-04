using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "invoice", "section", "show")]
public record AzBillingInvoiceSectionShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--invoice-section-name")] string InvoiceSectionName,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions;