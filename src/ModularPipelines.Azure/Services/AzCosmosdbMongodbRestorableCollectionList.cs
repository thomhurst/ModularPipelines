using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "restorable-collection")]
public class AzCosmosdbMongodbRestorableCollectionList
{
    public AzCosmosdbMongodbRestorableCollectionList(
        AzCosmosdbMongodbRestorableCollectionListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbRestorableCollectionListCosmosdbPreview CosmosdbPreview { get; }
}