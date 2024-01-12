using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-reservations", "create")]
public record GcloudPubsubLiteReservationsCreateOptions(
[property: PositionalArgument] string Reservation,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--throughput-capacity")] string ThroughputCapacity
) : GcloudOptions;