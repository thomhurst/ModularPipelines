using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "rooms", "create")]
public record AzCommunicationRoomsCreateOptions : AzOptions
{
    [CliOption("--attendee-participants")]
    public string? AttendeeParticipants { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--consumer-participants")]
    public string? ConsumerParticipants { get; set; }

    [CliOption("--presenter-participants")]
    public string? PresenterParticipants { get; set; }

    [CliFlag("--pstn-dial-out-enabled")]
    public bool? PstnDialOutEnabled { get; set; }

    [CliOption("--valid-from")]
    public string? ValidFrom { get; set; }

    [CliOption("--valid-until")]
    public string? ValidUntil { get; set; }
}