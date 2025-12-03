using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "asn", "create")]
public record AzPeeringAsnCreateOptions(
[property: CliOption("--peer-asn-name")] string PeerAsnName
) : AzOptions
{
    [CliOption("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CliOption("--peer-contact-detail")]
    public string? PeerContactDetail { get; set; }

    [CliOption("--peer-name")]
    public string? PeerName { get; set; }

    [CliOption("--validation-state")]
    public string? ValidationState { get; set; }
}