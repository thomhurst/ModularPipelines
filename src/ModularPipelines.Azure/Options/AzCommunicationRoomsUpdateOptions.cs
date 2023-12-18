using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "rooms", "update")]
public record AzCommunicationRoomsUpdateOptions(
[property: CommandSwitch("--room")] string Room
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--pstn-dial-out-enabled")]
    public bool? PstnDialOutEnabled { get; set; }

    [CommandSwitch("--valid-from")]
    public string? ValidFrom { get; set; }

    [CommandSwitch("--valid-until")]
    public string? ValidUntil { get; set; }
}