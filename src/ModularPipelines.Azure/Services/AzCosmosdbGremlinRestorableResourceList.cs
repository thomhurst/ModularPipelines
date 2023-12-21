using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin", "restorable-resource")]
public class AzCosmosdbGremlinRestorableResourceList
{
    public AzCosmosdbGremlinRestorableResourceList(
        AzCosmosdbGremlinRestorableResourceListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbGremlinRestorableResourceListCosmosdbPreview CosmosdbPreview { get; }
}