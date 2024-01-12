using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-reservations", "update")]
public record GcloudPubsubLiteReservationsUpdateOptions(
[property: PositionalArgument] string Reservation,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--throughput-capacity")] string ThroughputCapacity
) : GcloudOptions;