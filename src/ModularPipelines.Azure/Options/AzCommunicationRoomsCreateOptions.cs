using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "rooms", "create")]
public record AzCommunicationRoomsCreateOptions(
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

    [BooleanCommandSwitch("--pstn-dial-out-enabled")]
    public bool? PstnDialOutEnabled { get; set; }

    [CommandSwitch("--valid-from")]
    public string? ValidFrom { get; set; }

    [CommandSwitch("--valid-until")]
    public string? ValidUntil { get; set; }
}