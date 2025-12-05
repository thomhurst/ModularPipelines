using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("capacity", "reservation", "group", "show")]
public record AzCapacityReservationGroupShowOptions(
[property: CliOption("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--instance-view")]
    public string? InstanceView { get; set; }
}