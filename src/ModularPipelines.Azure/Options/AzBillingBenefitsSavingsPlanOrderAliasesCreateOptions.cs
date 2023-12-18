using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan-order-aliases", "create")]
public record AzBillingBenefitsSavingsPlanOrderAliasesCreateOptions(
[property: CommandSwitch("--order-alias-name")] string OrderAliasName
) : AzOptions
{
    [CommandSwitch("--applied-scope-prop")]
    public string? AppliedScopeProp { get; set; }

    [CommandSwitch("--applied-scope-type")]
    public string? AppliedScopeType { get; set; }

    [CommandSwitch("--billing-plan")]
    public string? BillingPlan { get; set; }

    [CommandSwitch("--billing-scope-id")]
    public string? BillingScopeId { get; set; }

    [CommandSwitch("--commitment")]
    public string? Commitment { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--term")]
    public string? Term { get; set; }
}