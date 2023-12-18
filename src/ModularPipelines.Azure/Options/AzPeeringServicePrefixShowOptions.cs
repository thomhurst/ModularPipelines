using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "prefix", "show")]
public record AzPeeringServicePrefixShowOptions(
[property: CommandSwitch("--peering-service-name")] string PeeringServiceName,
[property: CommandSwitch("--prefix-name")] string PrefixName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}