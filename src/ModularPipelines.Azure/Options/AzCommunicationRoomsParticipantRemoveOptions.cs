using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "rooms", "participant", "remove")]
public record AzCommunicationRoomsParticipantRemoveOptions(
[property: CommandSwitch("--participants")] string Participants,
[property: CommandSwitch("--room")] string Room
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

