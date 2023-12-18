using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "location", "list")]
public record AzPeeringServiceLocationListOptions : AzOptions
{
    [CommandSwitch("--country")]
    public int? Country { get; set; }
}