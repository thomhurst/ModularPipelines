using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "user", "definition")]
public class AzCosmosdbMongodbUserDefinitionUpdate
{
    public AzCosmosdbMongodbUserDefinitionUpdate(
        AzCosmosdbMongodbUserDefinitionUpdateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbUserDefinitionUpdateCosmosdbPreview CosmosdbPreview { get; }
}