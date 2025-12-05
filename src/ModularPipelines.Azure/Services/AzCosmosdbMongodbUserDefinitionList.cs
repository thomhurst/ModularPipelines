using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "user", "definition")]
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