using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "rooms", "delete")]
public record AzCommunicationRoomsDeleteOptions(
[property: CommandSwitch("--room")] string Room
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}