using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "datacenter")]
public class AzManagedCassandraDatacenterUpdate
{
    public AzManagedCassandraDatacenterUpdate(
        AzManagedCassandraDatacenterUpdateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraDatacenterUpdateCosmosdbPreview CosmosdbPreview { get; }
}