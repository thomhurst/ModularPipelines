using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "registered-asn", "delete")]
public record AzPeeringRegisteredAsnDeleteOptions(
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--registered-asn-name")] string RegisteredAsnName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;