using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "cluster")]
public class AzManagedCassandraClusterList
{
    public AzManagedCassandraClusterList(
        AzManagedCassandraClusterListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraClusterListCosmosdbPreview CosmosdbPreview { get; }
}