using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "order-item", "create")]
public record AzEdgeorderOrderItemCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--order-item-resource")] string OrderItemResource,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}