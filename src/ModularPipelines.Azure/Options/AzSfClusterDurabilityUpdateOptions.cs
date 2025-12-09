using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "cluster", "durability", "update")]
public record AzSfClusterDurabilityUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--durability-level")] string DurabilityLevel,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;