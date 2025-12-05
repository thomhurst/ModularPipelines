using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "asn", "delete")]
public record AzPeeringAsnDeleteOptions(
[property: CliOption("--peer-asn-name")] string PeerAsnName
) : AzOptions;