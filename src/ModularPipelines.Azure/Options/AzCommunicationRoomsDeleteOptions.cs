using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "rooms", "delete")]
public record AzCommunicationRoomsDeleteOptions(
[property: CliOption("--room")] string Room
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}