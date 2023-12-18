using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "location", "list")]
public record AzPeeringLocationListOptions(
[property: CommandSwitch("--kind")] string Kind
) : AzOptions
{
    [CommandSwitch("--direct-peering-type")]
    public string? DirectPeeringType { get; set; }
}