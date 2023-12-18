using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "location", "list")]
public record AzPeeringServiceLocationListOptions : AzOptions
{
    [CommandSwitch("--country")]
    public int? Country { get; set; }
}