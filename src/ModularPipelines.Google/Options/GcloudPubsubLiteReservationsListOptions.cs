using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-reservations", "list")]
public record GcloudPubsubLiteReservationsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;