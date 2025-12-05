using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("capacity", "reservation", "show")]
public record AzCapacityReservationShowOptions(
[property: CliOption("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CliOption("--capacity-reservation-name")] string CapacityReservationName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--instance-view")]
    public string? InstanceView { get; set; }
}