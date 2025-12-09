using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "cluster")]
public class AzManagedCassandraClusterShow
{
    public AzManagedCassandraClusterShow(
        AzManagedCassandraClusterShowCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraClusterShowCosmosdbPreview CosmosdbPreview { get; }
}