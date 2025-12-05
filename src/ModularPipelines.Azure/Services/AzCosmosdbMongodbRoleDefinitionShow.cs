using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "role", "definition")]
public class AzCosmosdbMongodbRoleDefinitionShow
{
    public AzCosmosdbMongodbRoleDefinitionShow(
        AzCosmosdbMongodbRoleDefinitionShowCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbRoleDefinitionShowCosmosdbPreview CosmosdbPreview { get; }
}