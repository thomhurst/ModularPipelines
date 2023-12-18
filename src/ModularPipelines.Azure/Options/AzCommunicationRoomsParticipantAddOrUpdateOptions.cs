using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "rooms", "participant", "add-or-update")]
public record AzCommunicationRoomsParticipantAddOrUpdateOptions(
[property: CommandSwitch("--room")] string Room
) : AzOptions
{
    [CommandSwitch("--attendee-participants")]
    public string? AttendeeParticipants { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--consumer-participants")]
    public string? ConsumerParticipants { get; set; }

    [CommandSwitch("--presenter-participants")]
    public string? PresenterParticipants { get; set; }
}