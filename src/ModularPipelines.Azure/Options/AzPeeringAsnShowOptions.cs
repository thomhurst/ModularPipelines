using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "asn", "show")]
public record AzPeeringAsnShowOptions(
[property: CommandSwitch("--peer-asn-name")] string PeerAsnName
) : AzOptions;