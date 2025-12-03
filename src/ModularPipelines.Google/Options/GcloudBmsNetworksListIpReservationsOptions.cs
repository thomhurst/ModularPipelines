using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "networks", "list-ip-reservations")]
public record GcloudBmsNetworksListIpReservationsOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}