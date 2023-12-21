using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin", "restorable-graph")]
public class AzCosmosdbGremlinRestorableGraphList
{
    public AzCosmosdbGremlinRestorableGraphList(
        AzCosmosdbGremlinRestorableGraphListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbGremlinRestorableGraphListCosmosdbPreview CosmosdbPreview { get; }
}