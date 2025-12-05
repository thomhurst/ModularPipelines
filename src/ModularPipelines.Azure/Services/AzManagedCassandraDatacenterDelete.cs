using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "datacenter")]
public class AzManagedCassandraDatacenterDelete
{
    public AzManagedCassandraDatacenterDelete(
        AzManagedCassandraDatacenterDeleteCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraDatacenterDeleteCosmosdbPreview CosmosdbPreview { get; }
}