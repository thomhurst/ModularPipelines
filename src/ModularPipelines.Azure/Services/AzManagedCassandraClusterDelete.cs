using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "cluster")]
public class AzManagedCassandraClusterDelete
{
    public AzManagedCassandraClusterDelete(
        AzManagedCassandraClusterDeleteCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraClusterDeleteCosmosdbPreview CosmosdbPreview { get; }
}