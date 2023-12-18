using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan-order", "savings-plan", "update")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanUpdateOptions(
[property: CommandSwitch("--savings-plan-id")] string SavingsPlanId,
[property: CommandSwitch("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CommandSwitch("--applied-scope-prop")]
    public string? AppliedScopeProp { get; set; }

    [CommandSwitch("--applied-scope-type")]
    public string? AppliedScopeType { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--renew")]
    public bool? Renew { get; set; }

    [CommandSwitch("--renew-properties")]
    public string? RenewProperties { get; set; }
}