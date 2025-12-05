using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "datacenter")]
public class AzManagedCassandraDatacenterCreate
{
    public AzManagedCassandraDatacenterCreate(
        AzManagedCassandraDatacenterCreateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzManagedCassandraDatacenterCreateCosmosdbPreview CosmosdbPreview { get; }
}