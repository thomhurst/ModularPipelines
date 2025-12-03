using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-reservations", "update")]
public record GcloudPubsubLiteReservationsUpdateOptions(
[property: CliArgument] string Reservation,
[property: CliArgument] string Location,
[property: CliOption("--throughput-capacity")] string ThroughputCapacity
) : GcloudOptions;