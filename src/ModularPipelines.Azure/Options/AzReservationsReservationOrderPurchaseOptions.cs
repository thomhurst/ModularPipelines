using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation-order", "purchase")]
public record AzReservationsReservationOrderPurchaseOptions(
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CommandSwitch("--applied-scope")]
    public string? AppliedScope { get; set; }

    [CommandSwitch("--applied-scope-property")]
    public string? AppliedScopeProperty { get; set; }

    [CommandSwitch("--applied-scope-type")]
    public string? AppliedScopeType { get; set; }

    [CommandSwitch("--billing-plan")]
    public string? BillingPlan { get; set; }

    [CommandSwitch("--billing-scope")]
    public string? BillingScope { get; set; }

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