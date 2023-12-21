using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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