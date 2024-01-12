using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-reservations", "list")]
public record GcloudPubsubLiteReservationsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;