using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("capacity", "reservation", "group", "create")]
public record AzCapacityReservationGroupCreateOptions(
[property: CliOption("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}