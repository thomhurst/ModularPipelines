using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("reservations", "reservation", "update")]
public record AzReservationsReservationUpdateOptions(
[property: CliOption("--reservation-id")] string ReservationId,
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliOption("--applied-scope-property")]
    public string? AppliedScopeProperty { get; set; }

    [CliOption("--applied-scope-type")]
    public string? AppliedScopeType { get; set; }

    [CliOption("--applied-scopes")]
    public string? AppliedScopes { get; set; }

    [CliOption("--instance-flexibility")]
    public string? InstanceFlexibility { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--renew")]
    public bool? Renew { get; set; }

    [CliOption("--renewal-properties")]
    public string? RenewalProperties { get; set; }
}