using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "subscription", "update")]
public record AzBillingSubscriptionUpdateOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--cost-center")]
    public string? CostCenter { get; set; }

    [CliOption("--sku-id")]
    public string? SkuId { get; set; }

    [CliOption("--subscription-billing-status")]
    public string? SubscriptionBillingStatus { get; set; }
}