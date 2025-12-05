using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "transaction", "list")]
public record AzBillingTransactionListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--invoice-name")] string InvoiceName
) : AzOptions;