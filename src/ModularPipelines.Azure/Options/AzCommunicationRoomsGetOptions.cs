using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "rooms", "get")]
public record AzCommunicationRoomsGetOptions(
[property: CliOption("--room")] string Room
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}