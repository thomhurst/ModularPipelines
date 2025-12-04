using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "rooms", "participant", "remove")]
public record AzCommunicationRoomsParticipantRemoveOptions(
[property: CliOption("--participants")] string Participants,
[property: CliOption("--room")] string Room
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}