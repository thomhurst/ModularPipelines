using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "rooms", "update")]
public record AzCommunicationRoomsUpdateOptions(
[property: CliOption("--room")] string Room
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--pstn-dial-out-enabled")]
    public bool? PstnDialOutEnabled { get; set; }

    [CliOption("--valid-from")]
    public string? ValidFrom { get; set; }

    [CliOption("--valid-until")]
    public string? ValidUntil { get; set; }
}