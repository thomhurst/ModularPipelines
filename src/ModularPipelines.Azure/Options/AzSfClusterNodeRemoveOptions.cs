using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "cluster", "node", "remove")]
public record AzSfClusterNodeRemoveOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--nodes-to-remove")] string NodesToRemove,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;