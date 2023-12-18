using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "subscription", "update")]
public record AzBillingSubscriptionUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--cost-center")]
    public string? CostCenter { get; set; }

    [CommandSwitch("--sku-id")]
    public string? SkuId { get; set; }

    [CommandSwitch("--subscription-billing-status")]
    public string? SubscriptionBillingStatus { get; set; }
}