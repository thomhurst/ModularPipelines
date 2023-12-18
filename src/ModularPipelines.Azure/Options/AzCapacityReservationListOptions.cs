using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("capacity", "reservation", "list")]
public record AzCapacityReservationListOptions(
[property: CommandSwitch("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;