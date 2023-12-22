using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "asn", "delete")]
public record AzPeeringAsnDeleteOptions(
[property: CommandSwitch("--peer-asn-name")] string PeerAsnName
) : AzOptions;