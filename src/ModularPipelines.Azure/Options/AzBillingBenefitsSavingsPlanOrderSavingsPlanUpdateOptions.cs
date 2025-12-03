using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan-order", "savings-plan", "update")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanUpdateOptions(
[property: CliOption("--savings-plan-id")] string SavingsPlanId,
[property: CliOption("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CliOption("--applied-scope-prop")]
    public string? AppliedScopeProp { get; set; }

    [CliOption("--applied-scope-type")]
    public string? AppliedScopeType { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--renew")]
    public bool? Renew { get; set; }

    [CliOption("--renew-properties")]
    public string? RenewProperties { get; set; }
}