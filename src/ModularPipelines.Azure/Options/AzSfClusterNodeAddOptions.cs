using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "node", "add")]
public record AzSfClusterNodeAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--node-type")] string NodeType,
[property: CommandSwitch("--nodes-to-add")] string NodesToAdd,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}