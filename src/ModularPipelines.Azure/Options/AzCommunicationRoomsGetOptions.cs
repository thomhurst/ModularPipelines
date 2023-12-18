using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "rooms", "get")]
public record AzCommunicationRoomsGetOptions(
[property: CommandSwitch("--room")] string Room
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}