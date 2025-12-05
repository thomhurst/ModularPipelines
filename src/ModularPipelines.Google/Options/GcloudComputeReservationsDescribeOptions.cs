using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "reservations", "describe")]
public record GcloudComputeReservationsDescribeOptions(
[property: CliArgument] string Reservation,
[property: CliArgument] string Zone
) : GcloudOptions;