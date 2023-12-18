using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "subscription", "list")]
public record AzBillingSubscriptionListOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--customer-name")]
    public string? CustomerName { get; set; }

    [CommandSwitch("--invoice-section-name")]
    public string? InvoiceSectionName { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }
}