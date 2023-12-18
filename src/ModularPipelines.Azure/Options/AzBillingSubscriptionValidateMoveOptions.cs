using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "subscription", "validate-move")]
public record AzBillingSubscriptionValidateMoveOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--destination-invoice-section-id")] string DestinationInvoiceSectionId
) : AzOptions
{
}