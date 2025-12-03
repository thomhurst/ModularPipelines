using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "asn", "show")]
public record AzPeeringAsnShowOptions(
[property: CliOption("--peer-asn-name")] string PeerAsnName
) : AzOptions;