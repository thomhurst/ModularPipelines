using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "invoice", "section", "show")]
public record AzBillingInvoiceSectionShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--invoice-section-name")] string InvoiceSectionName,
[property: CommandSwitch("--profile-name")] string ProfileName
) : AzOptions
{
}