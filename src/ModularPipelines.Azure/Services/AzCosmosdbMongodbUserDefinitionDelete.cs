using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "user", "definition")]
public class AzCosmosdbMongodbUserDefinitionDelete
{
    public AzCosmosdbMongodbUserDefinitionDelete(
        AzCosmosdbMongodbUserDefinitionDeleteCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbUserDefinitionDeleteCosmosdbPreview CosmosdbPreview { get; }
}