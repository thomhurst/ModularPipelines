using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "datacenter")]
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