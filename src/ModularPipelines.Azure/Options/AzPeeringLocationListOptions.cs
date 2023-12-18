using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "location", "list")]
public record AzPeeringLocationListOptions(
[property: CommandSwitch("--kind")] string Kind
) : AzOptions
{
    [CommandSwitch("--direct-peering-type")]
    public string? DirectPeeringType { get; set; }
}