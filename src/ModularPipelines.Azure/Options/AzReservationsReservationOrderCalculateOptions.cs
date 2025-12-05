using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "reservation-order", "calculate")]
public record AzReservationsReservationOrderCalculateOptions : AzOptions
{
    [CliOption("--applied-scope")]
    public string? AppliedScope { get; set; }

    [CliOption("--applied-scope-property")]
    public string? AppliedScopeProperty { get; set; }

    [CliOption("--applied-scope-type")]
    public string? AppliedScopeType { get; set; }

    [CliOption("--billing-plan")]
    public string? BillingPlan { get; set; }

    [CliOption("--billing-scope")]
    public string? BillingScope { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--instance-flexibility")]
    public string? InstanceFlexibility { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--quantity")]
    public string? Quantity { get; set; }

    [CliFlag("--renew")]
    public bool? Renew { get; set; }

    [CliOption("--reserved-resource-type")]
    public string? ReservedResourceType { get; set; }

    [CliOption("--review-date-time")]
    public string? ReviewDateTime { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--term")]
    public string? Term { get; set; }
}