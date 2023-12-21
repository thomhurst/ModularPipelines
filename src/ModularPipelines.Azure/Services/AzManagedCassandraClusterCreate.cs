using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "cluster")]
public class AzManagedCassandraClusterCreate
{
    public AzManagedCassandraClusterCreate(
        AzManagedCassandraClusterCreateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraClusterCreateCosmosdbPreview CosmosdbPreview { get; }
}