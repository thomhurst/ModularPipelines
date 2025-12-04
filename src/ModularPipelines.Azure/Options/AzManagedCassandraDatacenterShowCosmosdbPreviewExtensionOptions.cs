using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managed-cassandra", "datacenter", "show", "(cosmosdb-preview", "extension)")]
public record AzManagedCassandraDatacenterShowCosmosdbPreviewExtensionOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--data-center-name")] string DataCenterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;