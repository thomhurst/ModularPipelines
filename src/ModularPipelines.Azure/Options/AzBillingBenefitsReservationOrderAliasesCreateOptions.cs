using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "reservation-order-aliases", "create")]
public record AzBillingBenefitsReservationOrderAliasesCreateOptions(
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

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--instance-flexibility")]
    public string? InstanceFlexibility { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--quantity")]
    public string? Quantity { get; set; }

    [BooleanCommandSwitch("--renew")]
    public bool? Renew { get; set; }

    [CommandSwitch("--reserved-resource-type")]
    public string? ReservedResourceType { get; set; }

    [CommandSwitch("--review-date-time")]
    public string? ReviewDateTime { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--term")]
    public string? Term { get; set; }
}

