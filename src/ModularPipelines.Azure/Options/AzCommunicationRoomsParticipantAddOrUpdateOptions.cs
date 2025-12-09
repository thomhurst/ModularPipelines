using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "rooms", "participant", "add-or-update")]
public record AzCommunicationRoomsParticipantAddOrUpdateOptions(
[property: CliOption("--room")] string Room
) : AzOptions
{
    [CliOption("--attendee-participants")]
    public string? AttendeeParticipants { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--consumer-participants")]
    public string? ConsumerParticipants { get; set; }

    [CliOption("--presenter-participants")]
    public string? PresenterParticipants { get; set; }
}