using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "cluster")]
public class AzManagedCassandraClusterDeallocate
{
    public AzManagedCassandraClusterDeallocate(
        AzManagedCassandraClusterDeallocateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraClusterDeallocateCosmosdbPreview CosmosdbPreview { get; }
}