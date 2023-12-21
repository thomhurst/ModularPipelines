using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
public class AzCosmosdbCassandra
{
    public AzCosmosdbCassandra(
        AzCosmosdbCassandraKeyspace keyspace,
        AzCosmosdbCassandraTable table
    )
    {
        Keyspace = keyspace;
        Table = table;
    }

    public AzCosmosdbCassandraKeyspace Keyspace { get; }

    public AzCosmosdbCassandraTable Table { get; }
}