using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("edgeorder", "order-item", "create")]
public record AzEdgeorderOrderItemCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--order-item-resource")] string OrderItemResource,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}