using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "update")]
public record AzPeeringServiceUpdateOptions(
[property: CommandSwitch("--peering-service-name")] string PeeringServiceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}