using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "datacenter", "list", "(cosmosdb-preview", "extension)")]
public record AzManagedCassandraDatacenterListCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;