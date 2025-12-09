using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "datacenter")]
public class AzManagedCassandraDatacenterShow
{
    public AzManagedCassandraDatacenterShow(
        AzManagedCassandraDatacenterShowCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraDatacenterShowCosmosdbPreview CosmosdbPreview { get; }
}