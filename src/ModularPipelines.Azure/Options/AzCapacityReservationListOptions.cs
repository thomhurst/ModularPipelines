using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("capacity", "reservation", "list")]
public record AzCapacityReservationListOptions(
[property: CliOption("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;