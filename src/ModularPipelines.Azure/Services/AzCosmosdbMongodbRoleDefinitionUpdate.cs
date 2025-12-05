using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "role", "definition")]
public class AzCosmosdbMongodbRoleDefinitionUpdate
{
    public AzCosmosdbMongodbRoleDefinitionUpdate(
        AzCosmosdbMongodbRoleDefinitionUpdateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbRoleDefinitionUpdateCosmosdbPreview CosmosdbPreview { get; }
}