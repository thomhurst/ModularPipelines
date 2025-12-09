using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "cluster", "node", "add")]
public record AzSfClusterNodeAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--nodes-to-add")] string NodesToAdd,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;