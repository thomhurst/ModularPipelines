using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "registered-prefix", "create")]
public record AzPeeringRegisteredPrefixCreateOptions(
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--registered-prefix-name")] string RegisteredPrefixName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }
}