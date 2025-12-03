using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-reservations", "delete")]
public record GcloudPubsubLiteReservationsDeleteOptions(
[property: CliArgument] string Reservation,
[property: CliArgument] string Location
) : GcloudOptions;