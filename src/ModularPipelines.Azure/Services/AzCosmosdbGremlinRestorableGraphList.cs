using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "gremlin", "restorable-graph")]
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