using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "user", "definition")]
public class AzCosmosdbMongodbUserDefinitionShow
{
    public AzCosmosdbMongodbUserDefinitionShow(
        AzCosmosdbMongodbUserDefinitionShowCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbUserDefinitionShowCosmosdbPreview CosmosdbPreview { get; }
}