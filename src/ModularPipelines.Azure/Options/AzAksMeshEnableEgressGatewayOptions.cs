using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "mesh", "enable-egress-gateway")]
public record AzAksMeshEnableEgressGatewayOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--egress-gateway-nodeselector")]
    public string? EgressGatewayNodeselector { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}