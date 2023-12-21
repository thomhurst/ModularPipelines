using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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