using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "prefix", "update")]
public record AzPeeringServicePrefixUpdateOptions(
[property: CommandSwitch("--peering-service-name")] string PeeringServiceName,
[property: CommandSwitch("--prefix-name")] string PrefixName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--peering-service-prefix-key")]
    public string? PeeringServicePrefixKey { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }
}