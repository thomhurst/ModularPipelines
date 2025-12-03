using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-reservations", "create")]
public record GcloudPubsubLiteReservationsCreateOptions(
[property: CliArgument] string Reservation,
[property: CliOption("--location")] string Location,
[property: CliOption("--throughput-capacity")] string ThroughputCapacity
) : GcloudOptions;