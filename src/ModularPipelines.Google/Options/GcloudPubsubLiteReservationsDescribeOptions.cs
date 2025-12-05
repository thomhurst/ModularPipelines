using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-reservations", "describe")]
public record GcloudPubsubLiteReservationsDescribeOptions(
[property: CliArgument] string Reservation,
[property: CliArgument] string Location
) : GcloudOptions;