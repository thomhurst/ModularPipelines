using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "transaction", "list")]
public record AzBillingTransactionListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--invoice-name")] string InvoiceName
) : AzOptions;