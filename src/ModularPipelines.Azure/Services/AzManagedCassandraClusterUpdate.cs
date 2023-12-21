using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "cluster")]
public class AzManagedCassandraClusterUpdate
{
    public AzManagedCassandraClusterUpdate(
        AzManagedCassandraClusterUpdateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraClusterUpdateCosmosdbPreview CosmosdbPreview { get; }
}