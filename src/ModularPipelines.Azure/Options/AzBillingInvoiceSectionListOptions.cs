using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "invoice", "section", "list")]
public record AzBillingInvoiceSectionListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions;