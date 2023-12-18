using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "legacy", "list")]
public record AzPeeringLegacyListOptions(
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--peering-location")] string PeeringLocation
) : AzOptions
{
    [CommandSwitch("--asn")]
    public string? Asn { get; set; }
}