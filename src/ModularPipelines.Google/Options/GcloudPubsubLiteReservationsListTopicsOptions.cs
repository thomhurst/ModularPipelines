using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-reservations", "list-topics")]
public record GcloudPubsubLiteReservationsListTopicsOptions(
[property: CliArgument] string Reservation,
[property: CliArgument] string Location
) : GcloudOptions;