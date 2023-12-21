using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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