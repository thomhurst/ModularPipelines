using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-reservations", "list-topics")]
public record GcloudPubsubLiteReservationsListTopicsOptions(
[property: PositionalArgument] string Reservation,
[property: PositionalArgument] string Location
) : GcloudOptions;