using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "user", "definition")]
public class AzCosmosdbMongodbUserDefinitionList
{
    public AzCosmosdbMongodbUserDefinitionList(
        AzCosmosdbMongodbUserDefinitionListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbUserDefinitionListCosmosdbPreview CosmosdbPreview { get; }
}