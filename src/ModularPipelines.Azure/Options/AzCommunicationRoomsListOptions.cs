using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "rooms", "list")]
public record AzCommunicationRoomsListOptions : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}