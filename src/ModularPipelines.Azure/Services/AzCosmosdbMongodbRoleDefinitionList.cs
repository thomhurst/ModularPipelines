using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "role", "definition")]
public class AzCosmosdbMongodbRoleDefinitionList
{
    public AzCosmosdbMongodbRoleDefinitionList(
        AzCosmosdbMongodbRoleDefinitionListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbRoleDefinitionListCosmosdbPreview CosmosdbPreview { get; }
}