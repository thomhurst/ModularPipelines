using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "datacenter")]
public class AzManagedCassandraDatacenterList
{
    public AzManagedCassandraDatacenterList(
        AzManagedCassandraDatacenterListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraDatacenterListCosmosdbPreview CosmosdbPreview { get; }
}