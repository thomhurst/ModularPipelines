using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "reservations", "describe")]
public record GcloudComputeReservationsDescribeOptions(
[property: PositionalArgument] string Reservation,
[property: PositionalArgument] string Zone
) : GcloudOptions;