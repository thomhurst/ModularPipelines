using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "networks", "list-ip-reservations")]
public record GcloudBmsNetworksListIpReservationsOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}