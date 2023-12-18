using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "rooms", "list")]
public record AzCommunicationRoomsListOptions : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}