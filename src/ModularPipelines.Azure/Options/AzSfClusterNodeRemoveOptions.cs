using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "node", "remove")]
public record AzSfClusterNodeRemoveOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--node-type")] string NodeType,
[property: CommandSwitch("--nodes-to-remove")] string NodesToRemove,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;