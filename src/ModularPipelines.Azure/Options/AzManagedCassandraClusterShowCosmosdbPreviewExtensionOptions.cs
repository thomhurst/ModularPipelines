using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managed-cassandra", "cluster", "show", "(cosmosdb-preview", "extension)")]
public record AzManagedCassandraClusterShowCosmosdbPreviewExtensionOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;