using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "user", "definition")]
public class AzCosmosdbMongodbUserDefinitionCreate
{
    public AzCosmosdbMongodbUserDefinitionCreate(
        AzCosmosdbMongodbUserDefinitionCreateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbUserDefinitionCreateCosmosdbPreview CosmosdbPreview { get; }
}