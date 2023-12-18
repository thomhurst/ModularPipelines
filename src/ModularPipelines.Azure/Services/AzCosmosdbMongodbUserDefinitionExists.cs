using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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