using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "user", "definition")]
public class AzCosmosdbMongodbUserDefinitionExists
{
    public AzCosmosdbMongodbUserDefinitionExists(
        AzCosmosdbMongodbUserDefinitionExistsCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbUserDefinitionExistsCosmosdbPreview CosmosdbPreview { get; }
}