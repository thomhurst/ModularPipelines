using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "user", "definition")]
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