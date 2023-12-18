using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "asn", "create")]
public record AzPeeringAsnCreateOptions(
[property: CommandSwitch("--peer-asn-name")] string PeerAsnName
) : AzOptions
{
    [CommandSwitch("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CommandSwitch("--peer-contact-detail")]
    public string? PeerContactDetail { get; set; }

    [CommandSwitch("--peer-name")]
    public string? PeerName { get; set; }

    [CommandSwitch("--validation-state")]
    public string? ValidationState { get; set; }
}

