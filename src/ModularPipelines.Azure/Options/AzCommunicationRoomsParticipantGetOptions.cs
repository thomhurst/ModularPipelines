using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "rooms", "participant", "get")]
public record AzCommunicationRoomsParticipantGetOptions(
[property: CliOption("--room")] string Room
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}