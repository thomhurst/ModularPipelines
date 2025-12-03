using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("capacity", "reservation", "delete")]
public record AzCapacityReservationDeleteOptions : AzOptions
{
    [CliOption("--capacity-reservation-group")]
    public string? CapacityReservationGroup { get; set; }

    [CliOption("--capacity-reservation-name")]
    public string? CapacityReservationName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}