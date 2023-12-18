using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin", "restorable-database")]
public class AzCosmosdbGremlinRestorableDatabaseList
{
    public AzCosmosdbGremlinRestorableDatabaseList(
        AzCosmosdbGremlinRestorableDatabaseListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbGremlinRestorableDatabaseListCosmosdbPreview CosmosdbPreview { get; }
}